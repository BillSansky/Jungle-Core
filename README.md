# Jungle Core

Jungle Core contains the shared editor utilities and UI assets that power the Jungle toolchain.
It is distributed as a Unity Package Manager (UPM) package so it can be added to any project
without copying files into your `Assets` folder.

## Package contents

* **Jungle Package Hub** – a custom editor window that lets you browse and install Jungle
  packages from git URLs.
* **Shared editor styles** – USS files that provide the look & feel used across Jungle tooling.
* **Samples** – a bootstrapper example that demonstrates how to trigger package installation
  automatically when a project opens.

## Installing

Add the package via git using the Package Manager window:

1. Open **Window ▸ Package Manager**.
2. Click the **+** button and choose **Add package from git URL…**.
3. Enter the repository URL, for example:

   ```text
   https://github.com/jungle-devs/Jungle-Core.git
   ```

Unity will download the package and make the `Jungle Package Hub` window available under
**Tools ▸ Jungle ▸ Package Hub**.

### Using the sample

Import the **Jungle Package Bootstrapper** sample from the package inspector to copy the
bootstrapper files into your project. After importing you can edit the generated
`upm-packages.json` file to list the git URLs that should be installed when the editor loads.
