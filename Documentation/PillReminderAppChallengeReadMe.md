# Pill Reminder App
The App is based as a solution to Tim Corey C# Challenge, but catains many more features and extensionblity

## Description
Despite the word "Pill Reminder App" the core of this project is a class libeary and with many abstraction 
implemented to allow cross-platform and different feature to be posible. 

### Model Folder
	
**Abstract Class**	
	
	TaskReminder
		
		Generic Abstract class for a reminder logic

**Interface**
	
	ITaskReminderIO
			
		Interface for persistence functions
		
	ITimeProvider
			
		Interface for time functions
		
	ITimer
		
		Interface for timer
**Struct**
		
	Time			
		
		A purely time value type that strip year,month,day 
			from the DateTime in C#,with method convert back 
			and forth to DateTime


### PillReminder Folder

	The pillReminder serve as one of the useage of the class libeary
	
**Model Folder**
	
	Pill
	
	Model for Pill with Guid, Name, Quantity to take, Description
	
	PillSchedule
	
	Model for single pill schedule, with a seperate schedule Guid, 
	pill it for, and a list of Time,Bool tuple for taken record
	
   **Persistence Folder**
		
		PillReminderIOJson
			
			PillReminder persistence function saving and retriving pill, pillSchedule 
			to JSON
		
		PillScheduleStorageObject
			
			A helper object that reduce complexity of the PillSchedule for the easy 
			IO operation for PillReminderIOJson

### Utlitie
	
	DateTimeProvider
		
		Provide DateTime infomation use C# DateTime
	
	DefaultTimer
		
		Timer class warp the C# timer 
			 



