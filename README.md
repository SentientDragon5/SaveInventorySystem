# Save Load and Inventory System
This is a demonstration of creating a save and load system to save and load player and world data to a binary files. This can be used to save all sorts of data, from inventories to transform data.
## The Inventory System
The inventory system has two parts, the frontend and the backend.
### Backend
The backend is organized in that it has ItemAssets (scriptableobjects) that you create that hold item data such as its use effects, its icon, etc.. In order to save this to a binary file, we cannot edit or save scriptable objects, so we create a key. The ItemKey holds references to each of the ItemAssets in a List. Now when we go to save, we save the type of item (its index on the key) and the amount of that item in the inventory. This is all done in the Inventory.cs file.
### Frontend
To create a UI that can display inventory information without loading the file everytime there is an edit to the inventory or you show the inventory, you have to have another organization of the inventory that is stored in a component. This can be easily written and read to create the frontend. I used a variety of methods to make the UI update not as often, including only updating on callbacks.
### UI
To create a UI that can be navigated by not just clicking I had to create the UISelect system. this uses distance and basicly raycasting to find the next button. The system included does not work very well, as this is the first system I have created on my own.
## Summary
To summarize, inventory data is in 4 states: 1 saved in binary - Save.cs converted using SaveUtil, 2 converted to be able to saved to binary (all primitive data types) - Inventory.cs and Save.cs, 3 in easy access for editing - Inventory.cs, in the UI - UISelect.cs

![alt text](https://github.com/SentientDragon5/SaveInventorySystem/blob/main/SaveLoadImg0.png?raw=true)
