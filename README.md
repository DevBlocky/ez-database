# EZ Database

Welcome to the Easy to use Database by BlockBa5her made for C#. This is a 1 file simple database, that is supposed to be fast, efficient, and easy to use. This is not supposed to be a thing that doesn't throw exceptions or is very thread safe so watch out. I do, however, try to prevent Read/Write exceptions throughout the file, to help out the noobs.

## How 2 use

### Step 1

Have a class, that is marked serializable, that you would like to store (You could also use a Tuple<> to store multiple variables)
**Example**:

```csharp
[Serializable]
public class MyItem
{
    public string MyText { get; set; }
}
```

You should be able to store this class inside of the database files easily.

### Step 2

*Create the database, and use it with the class*

Now, this part is actually pretty easy, you just write some code, create the class with the built in constructor, and then done!
**Example**:

```csharp
public static class Program
{
    public static void Main(string[] args)
    {
        var database = new Database("myFile.data", true); // creates the database, and also creates the file if not already created
        MyItem item = database.Read(); // Reading the database, returns dynamic which can be casted into the item of your choice
        database.Write(item); // Writing the item back into the database, again dynamic can be casted
    }
}
```

That was actually pretty easy. This should all help you safely save all of your information to that pesky file easily. This is supposed to be compact and light, so don't expect many major updates to this thing. Thnx.

## LICENSE

(sorry legal)

### Public Domain License

This software is available to everyone, free of charge, without warranty, of public domain (NO LICENSE).