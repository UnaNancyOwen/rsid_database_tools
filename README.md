rsid_database_tools
===================

This is split/merge/rename tools for database of RealSense ID viewer format.  

Tools
-----
### Split
```
rsid_split.exe ./db
```
```
./db1
./db2
./db3
```

### Merge
```
rsid_merge.exe ./db1 ./db2 ./db3
```
```
./db
```

### ReName
```
rsid_rename.exe ./db "Old User ID" "New User ID"
```
```
./db # contain renamed user
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
