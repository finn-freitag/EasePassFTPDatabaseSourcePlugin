# FTP/FTPS/SFTP Remote Database for Ease Pass

## Installation
This plugin could be installed in the latest version of Ease Pass using the plugin store in the settings.

For building it yourself, follow these steps:
- Clone the repository
- Verify the reference to EasePassExensibility.dll
- Verify the references in the Resouces.resx to the dependencies (Nuget).
- Build in release mode
- You can install the plugin using the "Add from file" button in the Ease Pass plugin manager.

Now the remote database plugin should appear in the "Remote Database Settings" section. Expand the UI to edit the remote access configuration like this:
```json
[
	{
		"Host": "123.123.2.123",
		"Port": 22,
		"Username": "your_ftp_username",
		"Password": "your_ftp_password",
		"RemotePath": "/your/remote/path/to/databasename.epdb",
		"Mode": "SFTP"
	}
]
```
The Mode could be "FTP", "FTPS" or "SFTP".

Make sure you've copied an existing database to the remote location!

Restart Ease Pass and your remote database is ready to use!
