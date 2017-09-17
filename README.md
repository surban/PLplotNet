# PLplot bindings for .Net

## Overview
PLplot is a cross-platform software package for creating scientific plots 
whose (UTF-8) plot symbols and text are limited in practice only by what 
Unicode-aware system fonts are installed on a user's computer.

The PLplot core library can be used to create standard x-y plots, 
semi-log plots, log-log plots, contour plots, 3D surface plots, mesh plots, 
bar charts and pie charts. Multiple graphs (of the same or different sizes) 
may be placed on a single page, and multiple pages are allowed for those 
device formats that support them.

Plot gallery: http://plplot.sourceforge.net/examples.php

Documentation: http://plplot.sourceforge.net/docbook-manual/plplot-html-5.13.0/

Output File Formats: PDF, PNG, JPEG, PostScript

Interactive Platforms: Windows GDI, GTK+, X11

## .Net bindings
This package provides a complete, but unofficial, .Net binding for PLplot.

Obtain the NuGet package from https://www.nuget.org/packages/PLplot/

The native libraries (x64) for Windows are included in this package.
For Linux PLplot must be installed on your system and libplplot.so must
be in your LD_LIBRARY_PATH.


## Usage
Create a PLPlot.PLStream object and call its instance methods.

See the examples in the `Samples` folder for more information.

