# MetalWeight Plugin for Rhino 5 and Matrix 9

![License: MIT](https://img.shields.io/badge/License-MIT-02c24f.svg)
![GitHub Release](https://img.shields.io/github/v/release/SkTamim/metal-weight-plugin?color=0f6afc)
![Build](https://img.shields.io/badge/build-Passing-02b82f)
![GitHub all releases](https://img.shields.io/github/downloads/SkTamim/metal-weight-plugin/total?color=c80afc)
![GitHub stars](https://img.shields.io/github/stars/SkTamim/metal-weight-plugin?style=social&color=yellow)





 > The **MetalWeight** is a powerful plugin for Rhino 5 and Matrix 9. It will help jewellery CAD designers to know the weight of any metal based on multiplication. (Like - 22k Gold, 18K Gold, Silver, etc.)

---

## 1. How to Use the Plugin 🚀

This guide explains how to install and run your custom `MetalWeight` plugin.

## 📦 Installation
1.  **Download the rhp file:** Download the latest release from the **[Releases](https://github.com/SkTamim/metal-weight-plugin/releases)** page.
2.  **Locate the Plugin File:** After downloading the file, find the `MetalWeight.rhp`.
3.  **Install in Rhino/Matrix:** Open Rhino 5 or Matrix 9. Drag and drop the `MetalWeight.rhp` file directly onto the main window or open `plugin manager` and install the plugin.
4.  **If you are still confused, go and watch the tutorial Video on Installation, [Click Here]()**

---

## How to Use

1.  **Start the Command:** Type the command `MetalWeight` into the Rhino command line and press **Enter**.
2.  **Select Objects:** The command is designed to work with pre-selected objects.
    * If you select your 3D models *before* running the command, it will use them automatically.
    * If you have nothing selected, the command will prompt you to "Select solids or meshes".
3.  **Enter Density:** After the objects are selected, the command line will prompt you to enter the density (the "multiplication number") for your chosen metal. The default value is `17.0`. Type your desired number and press **Enter**.
4.  **View Results:** The plugin will calculate the total volume and estimated weight. The results will be printed to the Rhino command line history.
5.  4.  **To watch the demo video, [Click Here]()**

### Features

* **Validation System:** If the plugin detects any invalid, non-solid, or open objects, a warning message box will appear. It will list the number of problematic objects, select them in the viewport for you to inspect, and ask if you want to proceed with the calculation.
* **Performance:** The plugin is multi-threaded, so it can process a large number of objects very quickly on modern computers.

---

# Metal Multiplication Numbers for Jewellery CAD

| Karatage            | Multiplication Number | 
|---------------------|-----------------------|
| **24K Gold**        | 19                    | 
| **22K Gold**        | 17                    |
| **18K Gold**        | 16                    |
| **14K Gold**        | 13                    |
| **Fine Silver**     | 10                    | 
| **Sterling Silver** | 10.4                  |
| **Platinum 950**    | 21.45                 |
| **Palladium**       | 12                    |
 
---

## License

This is an open-source project.
