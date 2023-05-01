# RDBEd - DAT & RDB Editor

RDBEd is a database editor for RetroArch/Libretro. It can read and write RDB and DAT files.

![RDBEd](https://raw.githubusercontent.com/schellingb/RDBEd/master/README.png)

## Features
- Load and Save RDB Files (RetroArch/Libretro Database)
- Load and Save DAT Files (in clrmamepro format)
- Sort, filter and search functionality
- Copy and paste from and to multiple fields
- Built in webbrowser for looking up information
- Export modifications to extend [libretro-database](https://github.com/libretro/libretro-database) building
- Validation and data generation tools
- Customizable user interface

## Using

### Download
You can find a binary download under the [Releases page](https://github.com/schellingb/RDBEd/releases/latest).  
Just extract and run RDBEd.exe.

### Requirements
Requires [.NET Framework 4.5](https://dotnet.microsoft.com/download/dotnet-framework/thank-you/net452-web-installer) to be installed (included in Windows 8 and newer).

### Running with Mono (Linux/macOS)
To run RDBEd with Mono, install the packages 'mono-runtime' and 'libmono-system-windows-forms4.0-cil'.

## Menu

### File -> New File

### File -> Open File

### File -> Import File
Import an additional file over the current edited data according to a specified key field.  
Options:
- File: Select the file to import
- Merge Key: Select the key field to combine the two datasets
- Add entries without match as new
- Mark fields with conflicts during import (and keep both values)

### File -> Save

### File -> Save As

### File -> Export Modifications DAT
Save all modifications done in a session to a new DAT file.  
Options:
- Output DAT: The output file to write to
- Merge Key: The key field to export for all rows

### File -> Export DAT

### File -> Export RDB

### File -> Exit

### Tools -> Unify Meta Data with Equal Field
Unify all meta data fields of entries that share the exact same value of the key field.  
Fields merged: Genre, Users, Release, Rumble, Analog, Coop, EnhancementHW, Franchise, OriginalTitle, Developer, Publisher, Origin, Region, Tags  
Rows with conflicts during the process will be marked with a warning flag so they can be checked afterwards.  

### Tools -> Generate Region and Tags from Name
Fill out the region and tags field according to the [no-intro naming convention](https://datomatic.no-intro.org/stuff/The%20Official%20No-Intro%20Convention%20(20071030).pdf).  
Tags generated are 'Unlicensed', 'Pirate', 'Arcade', 'Demo', 'Beta', 'Sample', 'Prototype' and languages.

### Tools -> Create Delta DAT from Two Files
Create a new DAT file with all changes between the two selected input files (can be either RDB or DAT).

### Tools -> Dump RDB File
Export the raw contents of an RDB file to a human readable text file to analyze or compare.

### Validate -> Validate Unique Field
Will mark all values of a field that are not unique in the loaded dataset with a warning.

### Validate -> Validate Field Values
Fields can be validated with a regular expression.  
It also comes with a preset to validate SNES serial numbers.  
More presets could be added in the future.

### Validate -> Clear Warning Flags
Clears all warning flags (red cells) that have been set.

## Controls

### Visible Columns
Show and hide columns by right clicking the header.

### CTRL + Up / CTRL + Down
Move the editing cursor to the next row with an empty field.

## License
RDBEd is available under the [GPL-3.0 license](https://choosealicense.com/licenses/gpl-3.0/).  
If you are interested in parts of this program under a more permissive or public domain license, feel free to contact me.
