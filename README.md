# MetalWeight Rhino 5 Plugin: Project Documentation

This document provides a complete overview of the `MetalWeight` plugin, a custom tool for Rhino 5 and Matrix 9 created for jewelry CAD designers. It covers how to use the plugin, its features, the development process, and the final source code.

---

## 1. How to Use the Plugin 🚀

This guide explains how to install and run your custom `MetalWeight` plugin.

### Installation

1.  **Locate the Plugin File:** After building the project in Visual Studio, find the `MetalWeight.rhp` file in the project's output folder (e.g., `MetalWeight/bin/x64/Debug/`).
2.  **Unblock the File:** This is a crucial step. Right-click on the `MetalWeight.rhp` file, select **Properties**, check the **"Unblock"** box at the bottom of the General tab, and click **OK**. If you don't see the Unblock box, the file is not blocked.
3.  **Install in Rhino/Matrix:** Open Rhino 5 or Matrix 9. Drag and drop the unblocked `MetalWeight.rhp` file directly onto the main window.

### Running the Command

1.  **Start the Command:** Type the command `MetalWeight` into the Rhino command line and press **Enter**.
2.  **Select Objects:** The command is designed to work with pre-selected objects.
    * If you select your 3D models *before* running the command, it will use them automatically.
    * If you have nothing selected, the command will prompt you to "Select solids or meshes".
3.  **Enter Density:** After the objects are selected, the command line will prompt you to enter the density (the "multiplication number") for your chosen metal. The default value is `17.0`. Type your desired number and press **Enter**.
4.  **View Results:** The plugin will calculate the total volume and estimated weight. The results will be printed to the Rhino command line history.

### Features

* **Validation System:** If the plugin detects any invalid, non-solid, or open objects, a warning message box will appear. It will list the number of problematic objects, select them in the viewport for you to inspect, and ask if you want to proceed with the calculation.
* **Performance:** The plugin is multi-threaded, so it can process a large number of objects very quickly on modern computers.

---

## 2. Project Overview

* **Plugin Name:** MetalWeight
* **Purpose:** To provide a fast and efficient tool for calculating the estimated weight of 3D models based on their volume and a user-provided density.
* **Target Software:** Rhino 5 (64-bit) and Matrix 9
* **Development Environment:** Visual Studio 2017
* **Framework:** .NET Framework 4.5

---

## 3. Development Journey & Troubleshooting Log 🛠️

This plugin was created through a long process of development and debugging. Here is a summary of the key challenges and solutions.

### A. Initial Setup and Build Configuration

The first major challenge was correctly configuring Visual Studio to build a `.rhp` file compatible with Rhino 5.
* **Problem:** Initial builds failed or produced `.dll` files instead of `.rhp` files.
* **Solution:**
    1.  **Platform Target:** The project was set to build specifically for the **x64** platform, as Rhino 5 (64-bit) requires 64-bit plugins.
    2.  **.NET Framework:** We determined that **.NET 4.5** was the most compatible framework. The necessary Developer Pack was manually installed to make this option available in Visual Studio.
    3.  **Post-Build Event:** A post-build command was added to the project settings (`ren "$(TargetPath)" "$(TargetName).rhp"`) to automatically rename the output `.dll` to `.rhp`.

### B. Plugin Loading Errors

Once the plugin was building, the next hurdle was getting Rhino to load it without errors.
* **Problem:** The plugin would fail to load, often with an "initialization failed" error.
* **Solution:**
    1.  **Unblocking Files:** We discovered that Windows was blocking the `.rhp` file for security reasons. The solution was to right-click the file, go to **Properties**, and check the **Unblock** box.
    2.  **Dependencies:** We used Rhino's `PluginManager` to diagnose dependency issues and ensure all required libraries were available.
    3.  **Unique ID (GUID):** To prevent conflicts, we created a new, unique GUID for the plugin in the `AssemblyInfo.cs` file.

### C. Performance and User Experience

With the plugin loading and running, the focus shifted to performance and usability.
* **Problem:** The initial version of the code was slow ("lagged") when calculating the volume of many objects. The application would also freeze and show "(Not Responding)".
* **Solution:**
    1.  **Single-Pass Loop:** The code was optimized to process each object only once, performing validation and volume calculation in the same loop.
    2.  **User Feedback:** A **progress bar** and **wait cursor** were added to provide feedback during long calculations. This prevented the application from appearing frozen.
    3.  **Parallel Processing:** For maximum speed, the final version of the plugin uses a `Parallel.ForEach` loop. This allows the code to use multiple CPU cores to process objects simultaneously, resulting in a significant performance increase.
    4.  **Thread Safety:** Advanced tools like `ConcurrentBag` and `lock` statements were implemented to safely handle data from multiple threads.
    5.  **API Compatibility:** We encountered and fixed several compiler errors that were caused by using code from newer versions of Rhino (like `Command.WasCancelled` or `MouseCursor.Set`). We replaced these with the correct APIs for the Rhino 5 SDK.

---
