# Changelog

## 1.0.42.0
- PostgreSQL 10 Support
- Dump is now created with the port defined in the settings

## 1.0.41.0
- Create Type Exception fixed

## 1.0.40.0
- Task.Extension NUGET upgraded

## 1.0.39.0
- Treat warnings as errors activated

## 1.0.38.0
- Task.Extension NUGET downgrade to avoid errors with Npgsql

## 1.0.37.0
- Updated NUGET packages

## 1.0.33.0
 - Fixed ALTER TYPE in different schemas

## 1.0.32.0
 - Create minor version
 - Fixed export bug if last version was minor
 - Fixed add database delay
 - Fixed exception when no default database folder is selected
 - Changelog visually more appealing

## 1.0.31.0
 - PostgreSQL RULES are now supported
 
## 1.0.29.0
 - Ignorable Schemas can be specified
   - APE.PostgreSQL.Teamwork and APE.PostgreSQL.Test.Runner are automatically added
 - Log when executing files changed from "without errors" to "successfully"

## 1.0.28.0
 - GitLab Compilation error fixed

## 1.0.27.0
 - Setting TABLE COLUMN to nullable not recognized fixed

## 1.0.26.0
- Changelog visible in application
- Error while writing to stream fixed (hopefully)
- Option to open diff file when an error while exporting occurs

## 1.0.1.4
- UI improvements
- New message when pg dump does not create a dump
- Message box content can be copied

## 1.0.1.3
- schema order bug fixed

## 0.0.0.17
- readme fixed
- changelog fixed

## 0.0.0.16
- added changelog