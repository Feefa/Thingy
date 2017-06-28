# Thingy - Feefa's Utility Library

Setting up a Thingy.WebServerLite web sever

- Create a project (forms, service, console)
- Add references to
  - Castle.Core.dll
  - Castle.Windsor.dll
  - Thingy.Infrastructure.dll
  - Thingy.WebServerLite.dll (There is no actual dependency on this but it is an easy way to get it copied to the bin directory)
  - Thingy.WebServerLiteApi.dll
- Create a config folder at the top level of the project
- Add an XML config file to the project inside the config folder (e.g. WebSites.xml)
- Add configuration for the default web site

  <?xml version="1.0" encoding="utf-8" ?>
  <configuration>
    <components>
      <component id="WebSite">
        <parameters>
          <name>WebSite</name> <!-- NB. You need to set this to match the assembly containing the controllers -->
          <portNumber>8080</portNumber> <!-- Any port that's not already in use - could it be 80? Maybe -->
          <path>site</path><!-- The root file directory -->
          <IsDefault>True</IsDefault><!-- Should always be true for the first web site -->
        </parameters>
      </component>
    </components>
  </configuration>
  
- Add configuration for the user authentication provider (this could be in a different xml file)

  <configuration>
    <components>
      <component id="KnownUserFactory">
        <parameters>
          <defaultRoles>
            <array>
              <element>Player</element><!-- The default roles that a registered user gets -->
            </array>
          </defaultRoles>
          <users>
            <array>
              <!-- Any user who requires roles beyond the default should be added here -->
              <element>Admin,admin,Admin</element><!-- UserName,password,Role;Role;Role... -->
            </array>
          </users>
        </parameters>
      </component>    
    </components>
  </configuration>
  
- Set all configuration files to "Copy to Output Directory" = "Copy if newer" in the properties panel
- Add a class named WebServerLoggingService provider that implements IWebServerLoggingService provider
  - It can be implemented with empty methods to begin with
  - Logging may be performed by multiple threads. Make sure the logger is thread-safe.
- Create a class named DefaultController that descends from ControllerBase
- Add a folder named site to the project
- Add a file named index.html to the site folder. Populate it with a simple, test web page
  - Set index.html to "Copy to Output Directory" = "Copy if newer" in the properties panel
- Use the CastleContainer in Thingy.Infrastructure to resolve a refernce to IWebServer
- Call Start() on the IWebServer implementation
- Try it in a browser
  - Something like http://localhost:8080
  - You should see your index page
  
- Add a url reservation by opening a cmd window and entering
  - netsh http add urlacl url="http://\*:8080/" user= {machine/userid}    
    - You can remove one with http add urlacl url="http://\*:8080/" if you get it wrong
- Allow the port through the windows firewall
