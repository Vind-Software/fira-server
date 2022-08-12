import os
import click
import json
import time
import psycopg2
import bisect 

connection = False
updaterConfs = {}

@click.group(invoke_without_command=True)
def main():
    click.echo('Getting configurations.............', nl=False)
    getConfigurations()
    click.echo(click.style(' OK', fg='green'))
    
    click.echo('Attempting database connection.....', nl=False)
    if getDatabaseConnection() :
        click.echo(click.style(' OK', fg='green'))
    else :
        click.echo(click.style(' ERROR', fg='red'))
        exit()

    click.echo('Verifying baseline..................', nl=False)    
    if not ensureBaselineAvailability() :
        click.echo('Baseline not available. Halting.', nl=False)
        exit()
    
    time.sleep(1.5)
    click.clear()

    selectFunction()

@click.command()
@click.option('--function', type=click.Choice(['1', '2', '3'], case_sensitive=False), prompt='Select an option: \n1- Update to most recent version \n2- List available updates \n3- Exit \n')
def selectFunction(function):
    click.echo('')
    if function == '1':
        updateToMostRecentVersion()
        pass
    elif function == '2':
        listAvailableVersions()
    elif function == '3':
        pass

    if function != '3':
        if click.confirm('Do you want to execute another function?'):
                click.clear()
                selectFunction()
    
    click.echo('Bye')

def getConfigurations():
    configsFile = open('updater.conf', 'r')
    configs = json.load(configsFile)
    configsFile.close()
    global updaterConfs 
    updaterConfs = configs
    return configs

def getDatabaseConnection():
    if not updaterConfs :
        getConfigurations()
    
    try:
        global connection 
        connection = psycopg2.connect(host=updaterConfs['host'], port=updaterConfs['port'], dbname=updaterConfs['database'], user=updaterConfs['user'], password=updaterConfs['password'])
        return True
    except psycopg2.errors.OperationalError as e:
        return False

def ensureBaselineAvailability():
    baselineExists = False
    with connection.cursor() as cursor:
        sql = "SELECT count(*) FROM information_schema.tables WHERE table_schema = 'application' and table_name = 'schema_change_log'"
        cursor.execute(sql)
        baselineExists = (cursor.fetchone())[0] > 0
    
    if not baselineExists :
        click.echo(click.style(' OK', fg='yellow'))
        if click.confirm('Baseline structure not found. This usually means that the target DB is not initialized yet. Install baseline now?'):
            with click.progressbar(length=100, label='Applying baseline:') as progressBar:
                if applyBaseline(progressBar) :
                    click.echo(click.style('DONE', fg='green'))
                    baselineExists = True
                else :
                    click.echo(click.style('Changes Discarded', fg='yellow'))
    else:
        click.echo(click.style(' OK', fg='green'))
    
    return baselineExists

def applyBaseline(progressBar):
    with connection.cursor() as cursor:
        progressBar.update(5)
        
        baselineMetadataFile = open(updaterConfs['scripts_directory'] + '/baseline.json', 'r')
        baselineMetadata = json.load(baselineMetadataFile)

        baselineScriptFile = open(updaterConfs['scripts_directory'] + '/baseline.sql', 'r')
        progressBar.update(10)

        cursor.execute(baselineScriptFile.read())
        progressBar.update(100)
        click.echo('')
        if click.confirm('Baseline successfully applied. Commit changes on database and register log?'):
            writeChangeToLog('baseline', baselineMetadata['application-version'], 'baseline.sql', baselineMetadata['author'], baselineMetadata['description'])
            connection.commit()
            return True
        else:
            connection.rollback()
            return False

def writeChangeToLog(dbver, appver, scriptName, author, description):
    with connection.cursor() as cursor:
        sql = 'INSERT INTO application.schema_change_log ("databaseVersion", "applicationVersion", "script", "author", "description") VALUES (%s, %s, %s, %s, %s)'
        cursor.execute(sql, (dbver, appver, scriptName, author, description))

def listAvailableVersions():
    dbVersion = getLatestVersionApplied()
    click.echo('Current database version: '+ click.style(dbVersion, fg='green'))
    if dbVersion == 'baseline':
        dbVersion = 0
    else:
        dbVersion = int(dbVersion.replace('.', ''))

    noVersionFound = True
    click.echo('Available versions: ')
    directory = os.fsencode(updaterConfs['scripts_directory'])
    for file in os.listdir(directory):
        fileName = os.fsdecode(file)
        if (not fileName.endswith(".json")) or fileName == 'baseline.json':
            continue
        
        fileVersion = getVersionFromFileName(fileName)

        if fileVersion > dbVersion :
            click.echo('- ' + click.style((os.path.splitext(fileName)[0]).split('sc-')[1], fg='blue'))
            noVersionFound = False
    
    if noVersionFound:
        click.echo('No available updates found')

def getLatestVersionApplied():
    with connection.cursor() as cursor:
        sql = 'SELECT "databaseVersion" FROM application.schema_change_log ORDER BY "dateApplied" DESC LIMIT 1'
        cursor.execute(sql)
        return (cursor.fetchone())[0] 

def getVersionFromFileName(fileName):
    return int(((os.path.splitext(fileName)[0]).split('sc-')[1]).replace('.', ''))

def updateToMostRecentVersion():
    dbVersion = getLatestVersionApplied()
    click.echo('Current database version: '+ click.style(dbVersion, fg='green'))
    if dbVersion == 'baseline':
        dbVersion = 0
    else:
        dbVersion = int(dbVersion.replace('.', ''))

    directory = os.fsencode(updaterConfs['scripts_directory'])
    versionsToApply = []
    for file in os.listdir(directory):
        fileName = os.fsdecode(file)
        if (not fileName.endswith(".json")) or fileName == 'baseline.json':
            continue
        
        fileVersion = getVersionFromFileName(fileName)

        if fileVersion > dbVersion :
            bisect.insort(versionsToApply, fileVersion)
    
    if versionsToApply:
        latestVersion = applyVersionMask(versionsToApply[-1])
        click.echo('The following database version will be applied: ' + click.style(latestVersion, fg='blue'))
        if click.confirm('Proceed?'):
            with click.progressbar(versionsToApply, label='Executing scripts:') as progressBar:
                for version in progressBar:
                    applyVersion(version)
            
            if click.confirm('Scripts successfully executed. Commit changes and generate change logs?'):
                connection.commit()
                click.echo(click.style('DONE', fg='green'))
            else:
                click.echo('Changes not applied. Returning to previous version')
                connection.rollback()
    else:
        click.echo('Database already on most recent version available')
            
def applyVersionMask(versionInt):
    versionString = format(versionInt, '08')
    return '{}{}.{}{}.{}{}{}{}'.format(*versionString)

def applyVersion(versionInt):
    versionString = applyVersionMask(versionInt)

    scriptMetadataFile = open(updaterConfs['scripts_directory'] + '/sc-' + versionString + '.json', 'r')
    scriptMetadata = json.load(scriptMetadataFile)

    executeScript(versionString)

    writeChangeToLog(versionString, scriptMetadata['application-version'], 'sc-'+versionString+'.sql', scriptMetadata['author'], scriptMetadata['description'])

def executeScript(versionString):
    with connection.cursor() as cursor:
        versionScriptFile = open(updaterConfs['scripts_directory'] + '/sc-' + versionString + '.sql', 'r')
        cursor.execute(versionScriptFile.read())

if __name__ == '__main__':
    main()