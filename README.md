# HR Manage System Blazor
> This project is a client design task for my college course. The backend is implemented in C#, while the frontend development was unrestricted, which led me to choose a Blazor Web App. To enhance the user interface, I integrated Tailwind CSS. Although several features are still a work in progress, I plan to refine and polish every function moving forward.

## Requirements
- Linux / OSX: `Npm`,`.NET9.0`,`mariadb` or `mysql`
- Windows: `Node.js`,`NET9.0`,`mariadb`

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

- Ensure that you have the NuGet package `MySqlConnector` installed.