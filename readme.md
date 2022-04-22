# AppSettings Crawler

Recursively locate appsettings*.json files in a given directory, and print out the flattened key names from all files.  Values are not printed.

Format is Filename|FlattenedKey

## Example: 

Given the following appsettings.json file:

```
    { 
        "Section 1" : {
            "Setting 1": "Foo",
            "Setting 2": "Bar"
        },
        "Section 2": {
            "SubSection 1": {
                "Setting 3": "Baz"
            }
        }
    }
```

Print the following output

/path/to/appsetting.json|Section 1:Setting 1
/path/to/appsetting.json|Section 1:Setting 2
/path/to/appsetting.json|Section 2:SubSection 1:Setting 3
