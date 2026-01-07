# Unity UI Architecture – Dynamic & Scalable UI System
A robust, modular, and scalable UI framework for Unity, designed to manage full-screen views, popups, and navigation efficiently. Perfect for building any type of Unity game UI (2D, 3D, mobile, desktop) with a clean and maintainable architecture.
This architecture system have following features:
# Features
Dynamic Screen Management: Easily show/hide full-screen views and popups.
Popup Handling: Automatic handling of popups with optional dim background and outside click to close.
Navigation History: Go back and forward between screens effortlessly.
Scalable Architecture: Add new screens or popups without touching core logic.
Animation Support: Smooth fade-in/out animations for popups.
Event System: UnityEvents to hook custom logic on screen show/hide.
Prefab-Based: Create and reuse screens and popups as prefabs.
Type-Safe Access: Access screens and popups via generics for clean code.

# Scene Setup

Canvas & Panel Setup
Create a Canvas and set your desired resolution
Add a full-screen empty Panel as parent for all screens
Design Screens
Create required screens (Splash, Home, Profile, etc.)
Save each screen and popup as prefabs
Attach Classes
Add UIView, FullScreenView, or PopupView script to each prefab
Assign required fields and hook buttons to the UIManager
Example Screens Included

Splash Screen:
<img width="756" height="339" alt="Screenshot 2025-12-29 164451" src="https://github.com/user-attachments/assets/1864a329-6cb8-4148-ad58-7a872b564a5b" />
Create a Canvas set resolution according to your requirements.
1.1 Create a empty full screen panel as Parent for all the screens
1.2 Design your required screens ( Splash, Home etc)
1.3 Create Class views for each screen and popup, attach to the screen and assign required fields Use UI scene Splash, Home, Reward Popup and Profile screens as example.
1.4 Save your created full screens and popups as prefabs

Home Screen:
<img width="757" height="350" alt="Screenshot 2025-12-29 165139" src="https://github.com/user-attachments/assets/c7666fc2-bd8c-44e6-b4d8-b168b973356d" />

Add empty GameObject attach UIManager class 
2.1 Assign all the data in the Screen Data make sure to use proper Screen Type screen prefab and rest of the values.
2.2 For popup you need to select parent screen type as well if you need to show popup under parent screen.
How to Add Popup screen
show up the way to add popups under the parent screen
default splash or set default screen 


# Why use this Architecture:
1. A clean and scalable UI architecture for Unity projects
2. Built for real-world production-ready Unity games.
3. Handles screens and popups with a single UI manager.
4. Simple navigation system with back and history support.
5. Designed to keep UI code organized and maintainable.
6. Fully prefab-driven UI workflow.
7. Easy to plug into existing Unity projects.
8. Supports smooth UI transitions and popup animations.
9. Flexible popup system with dim background support.
10. Type-safe access to screens for cleaner code.
11. Event-driven UI lifecycle using UnityEvents.