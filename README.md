# ImageConventer

WPF aplication desinged in MVVM pattern which allows user to load image (.jpg, .bmp or .png) and convert it using algorithm which selects main color (highest value out of Red/Green/Blue) from each pixel. 

Application provides three options to accomplish this task:

* normally - using usual loops in C# 
* asynchronoulsy - dividing image (byte array) to several parts which are converting at the same time
* using Cpp - using algorithm written in Cpp via CLI
