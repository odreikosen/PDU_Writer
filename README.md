# PDU_Writer

### Step 1
1) Input a comma seperated list of addresses for PDUs. Include the beginning subnet address range in this list.
2) Enter the end range the subnet address of all PDUs.
![PDU_Writer](/Images/intro.png)
*Adding PDU addresses from 10.60.0.2-10.60.0.254 and 10.60.1.2-10.60.1.254*



### Step 2
There are two options for this step. 
#### Manually input amount of current PDUs in PIQ
1) The user can either manually input the current amount of PDUs in their Power IQ and adjust the relations between entities if need be (the application is preset to standard relations). The resulting output of this method will be an excel file with the new entities and relations that need to be added to the Power IQ data model along with a CSV of the added PDUs. 
2) Hit Lets Get Started!

![Manual Step](/Images/step1.png)
*Knowing there are 128 PDUs currently in my PIQ I can input 128 for PDUs*

#### Input PIQ data file
1) The user input their current Power IQ data model, which the program will search to find the current amount of PDUs. The resulting output will be the amended Power IQ data model with the new entities and relations added along with a CSV of the added PDUs. 
2) Hit Lets Get Started!

![Data File Step](/Images/step2.png)
*Selecting my PIQ data model for the program to search for the amount of PDUs*
