#! "netcoreapp2.1"
#load "DataContextGenerator.GetAppEntities.Core.csx"

//when running from VS Code, set your options here
//Author Mrdotnet
// Code Marvel Project
if (!Args.Any()){ 
	options.Output = "DataContext.GetAppEntities.cs";
    options.ConfigFilePath = @"appsettings.json";
    options.Namespace = "BusinessLogic";
	options.RepoName = "MarvelRepoDB";
    options.ConnectionStringName = "ConnectionStrings:MyDatabase";	
}


var generator = new PocosGenerator(options);

WriteLine($"Connecting to database: {zap_password(generator.ConnectionString)}");

if (generator.ReadSchema()){
	WriteLine("Read Schema ....");
	// Let's remove ignore for tables and views we need
    /*
		generator.Tables["tablename"].Ignore = false;
	*/
	
	/*
		// Tweak Schema
		generator.Tables["tablename"].Ignore = true;						// To ignore a table
		generator.Tables["tablename"].ClassName = "newname";				// To change the class name of a table
		generator.Tables["tablename"]["columnname"].Ignore = true;			// To ignore a column
		generator.Tables["tablename"]["columnname"].PropertyName="newname";	// To change the property name of a column
		generator.Tables["tablename"]["columnname"].PropertyType="bool";	// To change the property type of a column
	*/
	WriteLine(generator.Tables.Count);
	generator.GenerateClass();
}

WriteLine($"Attempting to write generated content to {options.Output}");
System.IO.File.WriteAllText(options.Output, generator.Content);
WriteLine("Finished.");