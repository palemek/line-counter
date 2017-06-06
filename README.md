# line counter

## Description
Its a program for counting lines in all files with specific extensions in specified directory recursively. It includes counting empty lines and commented lines.

## Usage
From command line:
- first argument is directory in which files will be counted
- all arguments starting with "." will be extensions to look for
- arguments starting with "!": program will skip files with names with that argument in it(can be usefull with generated files which are not created by user)
 
## Example

```c#

F:\somelocation\codelocation>LineCounter.exe F:\somediffrentlocation .cs .sln

Summary:                             total       .cs      .sln
>>files with searched extensions:    34        30         4
>>lines:                             2906      2818        88
>>   code lines:                     2188      2104        84
>>       (less then 3 characters):   525       525         0
>>   clear lines(spaces or tabs):    386       382         4
>>   comment lines                   332       332         0
>>size of files(kilobytes):          106       102         3

```

## Known problems/bugs

- spaces in folders names can be a problem
- there is some problem in spacing in output
