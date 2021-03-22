rsid_database_tools
===================

This is split/merge tool for database of RealSense ID viewer format.  

Split
-----
You can split to each user database files from one database file with following command.  
### Command
```
rsid_database_tools.exe ./multiusers_db -output_dir ./databases/
```
### Input
```
./multiusers_db
```
### Output
```
./databases/
    singleuser_db1
    singleuser_db2
    singleuser_db3
    ...
```

Merge
-----
You can merge to one database file from each user database files with following command.  
### Command
```
rsid_database_tools.exe ./databases/ -output_dir ./ -merge
```
### Input
```
./databases/
    singleuser_db1
    singleuser_db2
    singleuser_db3
    ...
```
### Output
```
./multiusers_db
```

Environment
-----------
* Windows 10
* .Net 5 SDK (or later)
* RealSense ID SDK v0.13.0 (or later)

License
-------
Copyright &copy; 2021 Tsukasa SUGIURA  
Distributed under the [MIT License](http://www.opensource.org/licenses/mit-license.php "MIT License | Open Source Initiative").

Contact
-------
* Tsukasa Sugiura  
    * <t.sugiura0204@gmail.com>  
    * <http://unanancyowen.com>  
