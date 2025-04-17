# HR Manage System Blazor
> This project is a client design task for my college course. The backend is implemented in C#, while the frontend development was unrestricted, which led me to choose a Blazor Web App. To enhance the user interface, I integrated Tailwind CSS. Although several features are still a work in progress, I plan to refine and polish every function moving forward.

## Requirements
- Linux / OSX: `Npm`,`.NET9.0`,`mariadb` or `mysql`
- Windows (Click the link to download installer): [Node.js](https://nodejs.org/dist/v22.14.0/node-v22.14.0-x64.msi), [NET9.0](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-9.0.203-windows-x64-installer), [mariadb](https://mirrors.tuna.tsinghua.edu.cn/mariadb///mariadb-11.7.2/winx64-packages/mariadb-11.7.2-winx64.msi)

## IDE
The first option is Rider since the project hasn't been tested on Visual Studio.

## For First Use

1. Open your terminal and navigate to the project directory:

   ```shell
   cd HRMSystemLiu.2025
   ```

2. Install the necessary npm packages:

   ```shell
   npm install
   ```

3. Build the CSS files:

   ```shell
   npm run buildcss
   ```

4. Execute the SQL script from data.sql. This script includes some example data for the tables.

5. If Nuget packages are not automatically installed, you can manually add `DocumentFormat.OpenXml` to `HRMSystemLiu.2025` and add `MySqlConnector` to `HRMSystemLiu.DAL`
