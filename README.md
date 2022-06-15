### Troubleshooting
If TodoList shows UnauthorizedAccess:
- edit the compiled app LTA-Holoapp/LTA-Holoapp/Package.appxmanifest to add
<uap:Capability Name="documentsLibrary"/>
<rescap:Capability Name="broadFileSystemAccess" />
and
IgnorableNamespaces="uap uap2 uap3 uap4 mp rescap mobile iot"
xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities"

