[🇷🇺 Russian version of README.MD file](../master/README-RU.md)

# ⏱️🧑‍💼 Program for solving the problem of calculating working hours and wages of employees of the enterprise

FSBEI BSU. The result of the implementation of cicoa laboratory work on the subject of "Software Engineering".

## Basis for development

The program is developed on the basis of the bachelor's curriculum in the direction of preparation: 03/09/03
Applied Informatics, profile: Information systems and technologies in management and the work program of the course "Software Engineering".

## Purpose of development

The program is designed to solve the problem of calculating the total number of hours worked
(accurate to the nearest quarter of an hour) and wages for each employee of the enterprise based on data received in
as a result of accounting for employee visits using special cards handed over every week and containing:
+ Distinguishing number of the employee (five-digit integer);
+ Hourly wages (real number);
+ Time worked on Monday, Tuesday, Wednesday, Thursday and Friday (four-digit integer of the form ННММ, where НН - hours, ММ - minutes).
Users can be employees of the organization's management apparatus.

## Program interface

In accordance with the recommendations for the used style of the graphical interface, described in the terms of reference for the design of the graphical interface
designed program was created using the Material Design style.
When creating screen forms, the library of controls "Material Design In XAML Toolkit" is used, which allows you to fully
implement the Material Design standard in the Windows Presentation Foundation platform.
When working with the program interface, a single application window is used, in which page navigation is performed.

List of screen forms:
+ Main screen, divided into two tabs, including lists of information about employees and cards of hours worked;
+ Forms for editing and adding information about employees and cards of worked hours;
+ Forms for viewing information about specific employees and cards of worked hours, implemented in the form of modal dialog boxes;
+ Form for viewing the workload diagram of employees according to the specified criteria for selecting cards;
+ Login form;
+ Form of confirmation of deletion, implemented as a modal dialog box.

All icon buttons are equipped with pop-up tips that briefly describe the action that occurs when this button is clicked.

## Dependencies
+ Fody https://github.com/Fody/Home/
+ Costura.Fody https://github.com/Fody/Costura
+ INotifyPropertyChanged.Fody https://github.com/Fody/PropertyChanged
+ NPOI https://github.com/tonyqus/npoi
+ Microsoft EntityFramework 6 https://github.com/dotnet/ef6
+ NPGSQL https://github.com/npgsql/npgsql
