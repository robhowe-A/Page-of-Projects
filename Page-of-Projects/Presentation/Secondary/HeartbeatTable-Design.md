<!-- 
Copyright (c) 2026 Robert A. Howell
Author: Robert A. Howell
Description: This design was created for the implementation of the heartbeat table component.
Created_Date: July 2026
Edited: 2026-07-17
-->

Description: This document is a design diagram for the table component's data organization flow. The designed structure provides an extensive data architecture aligning targeted data structures to represent entry division by year, month, and day time ranges. Simply, the diagram shows the data structure and how information moves through it.
Restrictions: You may not use this code in commercial applications, production environments, or for unauthorized purposes without explicit permission from the author.
Author: see document metadata


~~~ text
:DATA IS SORTEDSET
:LIST[YEARS][MONTHS][DATE]
:TABLE HEAD
:TABLE BODY

    //DO
    |:YEAR = DATE(CURRENT YEAR)
    |:YEARSET SORTEDSET<DATA(DATE=:YEAR)> = SUBSET SELECT LIST.TOSET()
    |
    |   //NEW TABLE ROW YEAR SUMMARY (COLLAPSABLE)
    |IF:YEAR == DATE(CURRENT YEAR)
    |  (COLLAPSED = FALSE)
    |
    |   DO
    |   |:MONTH = DATE(CURRENT MONTH)
    |   |:MONTHSET SORTEDSET<DATA(DATE=:MONTH)> = SUBSET SELECT YEARSET.MONTH.TOSET()
    |   |
    |   |   //NEW TABLE ROW MONTH SUMMARY (COLLAPSABLE)
    |   |IF:MONTH == DATE(CURRENT MONTH)
    |   |  (COLLAPSED = FALSE)
    |   |
    |   |   WHILE MONTHSET.LENGTH!=0
    |   |   |:DAY = DATE(CURRENT DAY)
    |   |   |:DAYSET SORTEDSET<DATA(DATE=:DAY)> = SUBSET SELECT MONTHSET.DAY.TOSET()
    |   |   |
    |   |   |   //NEW TABLE ROW DAY SUMMARY
    |   |   |IF:DAY == DATE(CURRENT DAY)
    |   |   |  (COLLAPSED = FALSE)
    |   |   |
    |   |   |   FOREACH DATE:DAY IN DAYSET
    |   |   |     //NEW TABLE ROWS
    |   |   |
    |   |
    |   |WHILE YEARSET.LENGTH!=0
    |
    |
    |While ! END OF DATA
~~~
