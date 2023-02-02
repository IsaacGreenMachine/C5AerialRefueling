<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]


<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project
This project simulates [aerial refueling][refuel-video] between C5 and KC-135 aircrafts.

![Real Refuel Image][refuel_im]

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With
* [![Python][Python]][python-url]

* [![Unity][Unity]][unity-url]

* [![ML-Agents][MLAgents]][mla-url]

* [![PyTorch][PyTorch]][pytorch-url]

<p align="right">(<a href="#readme-top">back to top</a>)</p>



## Running the Demo
Run the [Build.rar](https://github.com/IsaacGreenMachine/C5AerialRefueling/blob/main/Build.rar) file on your machine or open the [Aerial Refueling Test](https://github.com/IsaacGreenMachine/C5AerialRefueling/tree/main/Aerial%20Refueling%20Test) Folder in [Unity Hub](https://unity.com/download).




<p align="right">(<a href="#readme-top">back to top</a>)</p>

# CymStar Unity R&D Writeup

Hello, CymStar! 

Here is a document containing all of the information you’ll need regarding the **********************************************************C5 Aerial Refueling Boom Arm********************************************************** project as well as some useful information we learned about Unity that may help in your future ventures with the engine.

The document can be broken down into two sections:

# 💬 useful stuff for building physics sims in Unity

In this section, I’ll share some of the biggest issues we ran into and how we solved them. Hopefully, it will provide insights, save you time, and help you learn from our mistakes.

I’ll cover the basics, but a good way to get up and going in Unity is to check out the [**Unity Learn Suite**](https://learn.unity.com), where there are tutorials for **********************everything.********************** Including [**ML-Agents**](https://learn.unity.com/course/ml-agents-hummingbirds)!

## unity basics

## setup

to get unity running on your machine, you’ll likely use the [Unity Hub](https://unity.com/download). ![Untitled](https://docs.unity3d.com/2019.1/Documentation/uploads/Main/gs_hub_installs_screen2.png).

This allows you to create and manage Unity projects and versions with a visual interface. If you have our project downloaded on your machine, you should be able to open in with this tool.

## the editor

![Untitled](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/1eebb0d5-33a5-42b9-b24f-889d2b6e8034/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20230202%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20230202T031312Z&X-Amz-Expires=86400&X-Amz-Signature=65c59017536ecde10c41606eb532ef354e9763d06993190cadc9236e341048f0&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D%22Untitled.png%22&x-id=GetObject)

You’ll spend a lot of time looking at this screen: it’s the editor.

Basically:

A: useful menu options. pay special attention to the play button. It’s how to run your sim.

B: the “Hierarchy”. a list of everything that currently exists in the running environment

E : the “Inspector”. This is where all the information about an object goes: Scripts, properties, textures, etc.

F: this is where you can see all the files for the project

## thinking like unity

Everything in Unity is either an Asset or a Component.

- Assets are objects that exist in the environment. Most of the assets you’ll be dealing with are [GameObjects](https://docs.unity3d.com/ScriptReference/GameObject.html)
- Components live inside of assets. It’s what makes Assets do stuff. Some components include:
    - Scripts
    - Rigidbodies
    - Colliders
    - etc.

When GameObjects are created, they come with a “transform” Component. This is how Unity keeps track of its position, rotation, and scale. 

![Untitled](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/e0feea48-2224-4814-9499-ca6e15232fe4/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20230202%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20230202T031410Z&X-Amz-Expires=86400&X-Amz-Signature=6e01c389c045fc863c3f60a71321e8c91fdab9ced50d30a601aa8a8b6638e1db&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D%22Untitled.png%22&x-id=GetObject)

GameObjects can be nested, which helps group them together. It also locks their positions together (if a parent moves 10 units on the x axis, so will all its children)

![Untitled](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/a40e26fc-e96f-45ac-94b4-3da6bb730713/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20230202%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20230202T031432Z&X-Amz-Expires=86400&X-Amz-Signature=4587188cf51b53f62643bf3b6b8a5b93947aebc741b2bc6fdec857689ce343df&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D%22Untitled.png%22&x-id=GetObject)

## accessing stuff using scripts

say you want to change a property on a GameObject that the current script isn’t attached to

maybe you want to access a component on another game object? or even a variable within another script

Here are a few ways to accomplish this:

```json
// getting a collider component from the GameObject that the script is attached to
Collider c = GetComponent<Collider>();

// finding a GameObject in the scene based on its name
GameObject g = GameObject.Find("name");

// getting a GameObject in the current parent/child group
	GameObject p  = transform.parent
	// getting the first child of the current GameObject
	GameObject c = transform.GetChild(0)

// getting a variable (assuming it's public) from another script
	// creating an empty pointer to the script
	MyScriptNameHere s;
	// pointing the pointer to a specific instance of the script
	s = GameObject.Find("cube").GetComponent<MyScriptNameHere>();
	// accessing the variable 'myvariablename' from the script
	s.myvariablename += 1;	
```

## moving stuff in the engine

Unity tracks its movements using [Quaternions](https://en.wikipedia.org/wiki/Quaternion), but luckily for us, we can view its movements in 3 Dimensional vectors, of type “[Vector3](https://docs.unity3d.com/ScriptReference/Vector3.html)”

if you need to convert from Quaternions to Vector3, see [Transform.EulerAngles](https://docs.unity3d.com/ScriptReference/Transform-eulerAngles.html)

### local / world space

One thing you need to know before getting into movements is that there are 2 spaces Unity uses : Local, and World

- World Space is not relative to any object.
- Local Space is relative to the referenced GameObject.

See below as the Red/Green/Blue arrows for the object move. The object’s “forward” is always (0, 0, 1) relative to itself, but will change relative to world space.

![https://miro.medium.com/max/1400/1*TZ3roDYgDMW4ZQJhMTilBw.gif](https://miro.medium.com/max/1400/1*TZ3roDYgDMW4ZQJhMTilBw.gif)

For example:

```csharp
// moves the GameObject forward on the ************************world Z axis,************************ (0, 0, 1), regardless of its rotation
transform.position += Vector3.forward

// moves the GameObject forward on its **local Z axis**, (0, 0, 1), in whatever direction it faces
transform.position += transform.forward
```

Here are methods to convert from local to world space

[Transform.TransformPoint](https://docs.unity3d.com/ScriptReference/Transform.TransformPoint.html)

and world to local

[Transform.InverseTransformPoint](https://docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html)

There are 2 main schools of thought when it comes to movement in unity:

### position-based movement

we started here. 

position-based movement involves explicitly changing an object’s transform via a script using methods like:

 

```csharp
// moves the GameObject attached to the script up 1 unity and forward 1 unit.
transform.position += new Vector3(0, 1, 1)

// sets the position of the GameObject attached to the script.
transform.position = new Vector3(-0.245, 257, 13.6)
```

To get objects to stick together, either move both game objects simultaneously or nest them in a [parent/child relationship](https://www.notion.so/CymStar-Unity-R-D-Writeup-8a8cdf24d9aa4a8692890780e1da64a8)

There’s a pretty big caveat about positional-based movement: collisions.

Position-based movement forces an objects position to change, which is really bad for detecting when objects have collided with each other.

You *could* code your own physics and collisions system using positional-based movement, but if you need object collisions, it’s much better to use Unity’s physics-based movement.

### physics-based movement

Physics-based movement implements Unity’s built-in physics system. You will need two components for this type of movement:

### [Rigidbodies](https://docs.unity3d.com/ScriptReference/Rigidbody.html)

![Untitled](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/e9858b92-4219-4c86-8d67-3047aa8e660b/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20230202%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20230202T031602Z&X-Amz-Expires=86400&X-Amz-Signature=7e3f957a66097dd85f7363151b7da8efcf19271d4b3fc5c3256b830d80946bdd&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D%22Untitled.png%22&x-id=GetObject)

the rigidbody component is what allows you to add forces to and control physic properties of GameObjects. 

Rigidbodies come with gravity (toggle-able), linear and angular velocity, different forms of friction and resistances, the whole deal.

*****************************************************************NOTE: DO NOT NEST RIGIDBODIES!************************************************************** There may only be one rigid body per parent/child group. Otherwise, the physics will break. If you are looking a way to stick objects together, or move them relative to each other, check out the “joints” section below.

### [Colliders](https://docs.unity3d.com/ScriptReference/Collider.html)

![Untitled](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/6bd47a07-d454-4b70-8dda-6251334c425b/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20230202%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20230202T031638Z&X-Amz-Expires=86400&X-Amz-Signature=5ece841ab4a898afac3e5a65548ee63276dbbf45f7573c6c207bd51a220518ab&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D%22Untitled.png%22&x-id=GetObject)

collider components allow you to define a rigidbody’s shape, and control how it will interact with other colliders. Any colliders that are within a parent/child group (no matter the depth of nesting) with a rigid body will be used for that rigid body’s collisions.

Some important settings:

isTrigger: “triggers” are non-physical colliders. This allows for detecting when something has entered an area, without stopping the object with a collision. If you want objects to bump each other, they need ********not******** to be triggers.

There are different types of colliders (capsule, box, sphere, etc.)

Mesh colliders are meant to surround a mesh, **but often don’t work**. For our project, we surrounded the aircraft with box and capsule colliders.

### [Joints](https://docs.unity3d.com/Manual/Joints.html)

![https://d37oebn0w9ir6a.cloudfront.net/account_3188/hinge_jointv2_b9ece22acaea526a2b5add15a1451340.gif](https://d37oebn0w9ir6a.cloudfront.net/account_3188/hinge_jointv2_b9ece22acaea526a2b5add15a1451340.gif)

Since you can’t nest rigidbodies in parent/child groups, you’ll need to use **************joints************** to control how objects are stuck to each other

Joints connect two rigidbodies depending on the type of joint.

### [**ArticulatedBodies**](https://www.google.com/search?client=safari&rls=en&q=Unity+Articulated+bodes&ie=UTF-8&oe=UTF-8)

![Untitled](https://s3.us-west-2.amazonaws.com/secure.notion-static.com/8a75eb52-5ebf-4537-9aab-ecbadd0b4272/Untitled.png?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Credential=AKIAT73L2G45EIPT3X45%2F20230202%2Fus-west-2%2Fs3%2Faws4_request&X-Amz-Date=20230202T031708Z&X-Amz-Expires=86400&X-Amz-Signature=188984709ed171b546b5906603dfc73bb53df046eb8d70a4331500b167748e1b&X-Amz-SignedHeaders=host&response-content-disposition=filename%3D%22Untitled.png%22&x-id=GetObject)

ArticulatedBodies are a unity component that enable complex mechanical behavior like robotic arms and articulated joints.

Specifically, it is a way to have ********************nested rigidbodies********************.

ArticulatedBodies function very similarly to Rigidbodies, but must be nested in a Parent/Child group to work.

Here’s a great demo using Articulated Bodies:

[https://github.com/Unity-Technologies/articulations-robot-demo](https://github.com/Unity-Technologies/articulations-robot-demo)

 

## Unity “gotchas”

### Nesting Rigidbodies

Nesting rigidbodies will break Physics! 
You can only have ********one******** rigidbody per parent/child group.
If you need objects with rigidbodies to move together, use ************joints************ or switch from rigidbodies to ****************articulatedBodies****************

### Scale of parent/children transforms

When putting GameObjects in Parent/Child groups, beware the scale of the object!
A GameObject’s scale will affect all its children. It is best practice to create an empty GameObject at a (1, 1, 1) scale when moving objects via their transform.

### Mesh colliders

[mesh colliders](https://docs.unity3d.com/Manual/class-MeshCollider.html) are a very tempting way to handle collisions. They don’t work most of the time, though. They are worth a try, and worth a try again with the “convex” option enabled, but it is best to use box / capsule / sphere colliders when possible.

# 📋 documentation for the sim we built

Now that you’re a Unity Expert (🤣), here’s how we built the sim:

If you haven’t seen our presentation, we did an overview.

**Presentation Video:**

[C15 Demo Day.mkv](https://drive.google.com/file/d/1jbaDxlbrAM4Cz4LUJlsFwuOSOKwQyM-C/view?t=23m57s)

**Slides:**

[Final Presentation](https://docs.google.com/presentation/d/1Nvw4DBlbsszmDLo6KLTgtykaMlqNhEDkEGH8OXYLzFA/edit?usp=sharing)

## 🎮 Play Guide

Once the project is open in the Unity Editor, you can go ahead and start the sim!

### First, ensure that you are on the correct “Scene”:

- Navigate to the Scenes folder in your Assets Menu and ***double click*** the scene “Main Menu”.
    
    ![123.png](/images/SceneSelect.png)
    
- After clicking the scene, you will be on the Main Menu screen. Then, ***you must press the play button*** at the top of the editor, to start the sim.
    
    ![safasfasf.PNG](/images/TitleScreen.png)
    
    From the Main Menu, you can choose to start the simulation by hitting the Start button. **(Visit the Settings page first, this is where you will select what you want to control.)**
    

### When selecting the Controls button, you will be taken to a page with various control layouts, navigate by choosing a new tab at the top:

- Each tab has corresponding button layouts based on your input device.
    
    ![joystick.PNG](/images/joystick.png)
    

### Settings Menu:

- The first time you enter the settings menu, no values will be selected.
    
    ![setting.PNG](/images/setting.png)
    
- On the left side, you can select whether you want to control the C5, the KC135, and/or the Boom Arm. **(If left unselected, they will default to being controlled by AI)**
    
    ![123123.png](/images/ControlsSelect1.png)
    
- Once you have selected which planes you’d like to control, you can select the input for whichever input device you are using. **(If left blank, it will default to keyboard)**
    
    ![12312312123.png](/images/ControlsSelect2.png)
    
- You can toggle on and off Volumetric Clouds and/or fog, depending on system requirements. As well as change the time of day with the slider.
    
    ![safasfas.PNG](/images/ControlsSelect3.PNG.png)
    
- The values will be saved when you exit the settings menu (saved in PlayerPrefs). Upon return, the settings will be taken from PlayerPrefs (A class that stores Player preferences between game sessions) and will reload the settings you have chosen.

### After selecting your settings, you are ready to start the simulation! Return to the Main Menu and hit Start!

## 🎛️ Tweaking Values

Want to change how the sim handles, but don’t want to mess with the code?
We built much of the sim to be tweak-able without touching code. 

### 🛩️ To tweak the C5:

### movement

Select “c5” in RealInstance > c5. In the Inspector, the “Plane Movement” Script has many values

![Screen Shot 2023-01-31 at 10.34.30 AM.png](/images/PlaneMovement.png)

Roll Speed: the speed at which the plane rolls

Pitch Speed: the speed at which the plane pitches

Yaw Speed: the speed at which the plane yaws

Ang of Attack: Angle of Attack. At what angle the plane stays static vertically

Throttle Change: the speed at which the throttle amount changes

Throttle Speed: the effect the throttle has on the aircraft

Refueller Speed: the speed that the C5 thinks the KC135 is going

Lift : how much lift is applied when above the angle of attack

LR Speed: how much the aircraft moves to its left/right when rolling 

### mass

Select “c5” in RealInstance > c5. In the Inspector, the “RigidBody” Component has settings for mass.

![Screen Shot 2023-01-31 at 10.50.26 AM.png](/images/Rigidbody.png)

### ✈️ To tweak the KC135:

### movement

Select “KC135” in RealInstance > KC135. In the Inspector, the “Plane Movement Articulation body” Script has many values

![Screen Shot 2023-01-31 at 10.42.05 AM.png](/images/PlaneMovementAB.png)

Roll Speed: the speed at which the plane rolls

Pitch Speed: the speed at which the plane pitches

Yaw Speed: the speed at which the plane yaws

Ang of Attack: Angle of Attack. At what angle the plane stays static vertically

Lift : how much lift is applied when above the angle of attack

LR Speed: how much the aircraft moves to its left/right when rolling 

### mass

Select “KC135” in RealInstance > KC135. In the Inspector, the “Articulation Body” Component has settings for mass.

![Screen Shot 2023-01-31 at 10.52.10 AM.png](/images/AB.png)

### 🦾 To tweak the Boom Arm:

### movement

Select “Outer_Boom” in RealInstance > KC135 > Outer_Boom. In the Inspector, the “Boom Arm Movement” Script has many values

![Screen Shot 2023-01-31 at 2.25.03 PM.png](/images/BAM.png)

Roll Change Speed : how quickly the arm will roll

Roll Max : how far the arm can move left (boom pilot perspective)

Roll Min : how far the arm can move right (boom pilot perspective)

Pitch Change Speed : how quickly the arm will pitch

Pitch Max : how far the arm can move forward (boom pilot perspective)

Pitch Min : how far the arm can move backward (boom pilot perspective)

Extend Speed : how quickly the arm will extend/retract

Extend Min : how far the arm may retract

Extend Max : how far the arm may extend

Fuel Rate : how quickly fuel passes from the KC135 to the C5

### mass

************Boom Arm************ 

Select “Outer_Boom” in RealInstance > KC135 > Outer_Boom. In the Inspector, the “Articulation Body” Component has mass options

![Screen Shot 2023-01-31 at 10.55.54 AM.png](/images/ArmAB.png)

********************Fuel Hose:********************

Select “Inner_Hose” in RealInstance > KC135 > Outer_Boom > Inner_Hose. In the Inspector, the “Articulation Body” Component has mass options

![Screen Shot 2023-01-31 at 10.58.20 AM.png](/images/HoseAB.png)

## 👷 Architecture

## 🧠 Neural Network Usage & Further Training

The Neural Network file is a [.onnx (Open Neural Network Exchange)](https://onnx.ai). 

The model needs input from the simulation. Based on this input, it will output values to control the arm. 

### ➡️ Model Input

Each input below is formatted as:

- [input number] Input Description (measurement or clarification) (type)
    - more details about input
    

The Model’s inputs are:

- [0-29] 5 [Ray Perception Sensors](https://docs.unity3d.com/Packages/com.unity.ml-agents@1.0/api/Unity.MLAgents.Sensors.RayPerceptionSensorComponent3D.html) that are cast from the nozzle.
    
    ![Screen Shot 2023-01-30 at 4.33.17 PM.png](/images/RayPerceptionSensors.png)
    
    - Each Ray Perception Sensor provides 6 inputs, for a total of 30 values.
    - Inputs are:
        - Whether ray has hit “Untagged” Tag (should never happen) (0 if no, 1 if yes) (int)
        - Whether ray has hit **“C5”** Tag (0 if no, 1 if yes) (int)
        - Whether ray has hit **“funnel”** Tag (0 if no, 1 if yes) (int)
        - Whether ray has hit **“hole”** Tag (0 if no, 1 if yes) (int)
        - Whether ray has not hit a known tag (0 if no, 1 if yes) (int)
        - Distance from Raycast point to Ray hit point (1 if ray does not hit) (float)
- [30] The distance from the nozzle of the boom arm to the C5’s fuel hole (meters) (float)
- [31, 32] the arm pitch and roll angular differences (degrees) (float)
    
    ![Screen Shot 2023-01-30 at 4.21.02 PM.png](/images/optimalAngleDiagram.png)
    
    - Vector 1 (current) from Rotation Point to Nozzle Tip
    - Vector 2 (ideal) from Rotation Point to fuel hole
    - pitch angle is POSITIVE if the arm is on the tail side of the fuel hole, NEGATIVE if on the nose side of the fuel hole
    - roll angle is POSITIVE if the arm is on the left side (from boom operator’s perspective) of hole, NEGATIVE if on right side (from boom operator’s perspective)
- [33] Amount of fuel delivered to the C5 (from 0 to 100) (float)
- [34] Clamped Status (0 / false if not clamped, 1 / true if clamped) (boolean)
- [35, 36, 37] X, Y, and Z coordinates of the nozzle tip (meters) (float)
    - measured from origin (0, 0, 0) of Unity world
- [38, 39, 40] X, Y, and Z coordinates of the fuel hole (meters) (float)
    - measured from origin (0, 0, 0) of Unity world
- [41] Empty Parameter (0) (float)
    - Due to a change we made very late in the training process, this parameter was removed, but is still needed for the model to function. A**lways pass 0 here.**

**All together, Input to the model should look like this:**

```json
DLRFU
[[RayPerceptionSensorDownHitUntagged,
  RayPerceptionSensorDownHitC5,
  RayPerceptionSensorDownHitfunnel,
	RayPerceptionSensorDownHithole,
	RayPerceptionSensorDownNoHit,
	RayPerceptionSensorDownHitDistance
 ],
 [RayPerceptionSensorLeftHitUntagged,
  RayPerceptionSensorLeftHitC5,
  RayPerceptionSensorLeftHitfunnel,
	RayPerceptionSensorLeftHithole,
	RayPerceptionSensorLeftNoHit,
	RayPerceptionSensorLeftHitDistance
 ],
 [RayPerceptionSensorRightHitUntagged,
  RayPerceptionSensorRightHitC5,
  RayPerceptionSensorRightHitfunnel,
	RayPerceptionSensorRightHithole,
	RayPerceptionSensorRightNoHit,
	RayPerceptionSensorRightHitDistance
 ],
 [RayPerceptionSensorFrontHitUntagged,
  RayPerceptionSensorFrontHitC5,
  RayPerceptionSensorFrontHitfunnel,
	RayPerceptionSensorFrontHithole,
	RayPerceptionSensorFrontNoHit,
	RayPerceptionSensorFrontHitDistance
 ],
 [RayPerceptionSensorUpHitUntagged,
  RayPerceptionSensorUpHitC5,
  RayPerceptionSensorUpHitfunnel,
	RayPerceptionSensorUpHithole,
	RayPerceptionSensorUpNoHit,
	RayPerceptionSensorUpHitDistance
 ],
NozzleDistanceFromFuelHole,
ArmPitchAngularDifference,
ArmRollAngularDifference,
FuelAmountDelivered,
ClampedStatus,
NozzleX,
NozzleY,
NozzleZ,
FuelHoleX,
FuelHoleY,
FuelHoleZ,
0]
```

### ⬅️ Model Output

The model outputs the following Values:

- Arm Pitch (between -1 and 1) (float)
    - negative value moves arm forward, positive value moves arm backward  (from boom arm pilot’s perspective)
- Arm Roll (between -1 and 1) (float)
    - negative value moves arm left, positive value moves arm right (from boom arm pilot’s perspective)
- Arm Extend/Retract (between -1 and 1) (float)
    - negative value retracts fuel hose, positive value extends fuel hose (from boom arm pilot’s perspective)
- Activate/Deactivate Clamp (0 or 1) (int)
    - “1” signifies pressing the “clamp” button, which will clamp the fuel hose to the fuel hole if it is correctly aligned, or unclamp the fuel hose from the fuel hole if it is currently clamped
    - “0” signifies not pressing the clamp button. When this value is 0, nothing will happen with the clamp.

Output from the neural network is divided into Continuous and Discrete actions:

```json
[[ArmPitchAmount,
  ArmRollAmount,
  ArmExtendAmount],
 [ActivateDeactivateClamp],
]
```

## 💪 Further Improvement and Training

Feel free to continue training the ONNX model in whatever way you see fit. It is a framework-agnostic neural network that can be used and trained withinmany tools. If you plan to continue training it with our setup (Unity + ML-Agents), I highly advise checking the Documentation to get setup and running.

[https://github.com/Unity-Technologies/ml-agents](https://github.com/Unity-Technologies/ml-agents)

Here is the trainer configuration .yaml file that we had the most success with:

### trainer_config.yaml

```json
behaviors:
  RefuelArm:
    trainer_type: ppo
    hyperparameters:
      batch_size: 2048
      buffer_size: 20480
      learning_rate: 0.0003
      beta: 0.005
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3
      learning_rate_schedule: linear
    network_settings:
      normalize: true
      hidden_units: 512
      num_layers: 3
      vis_encode_type: simple
    reward_signals:
      extrinsic:
        gamma: 0.995
        strength: 1.0
    keep_checkpoints: 5
    max_steps: 30000000
    time_horizon: 1000
    summary_freq: 30000
```

Here are some tips and tricks to improve Agent’s learning and performance:

### ⌛ Improving Training Speed

### Simulation Speed

To increase the simulation’s running speed while training, increase the time scale of the project to 20. Found at: 

Edit > Project Settings > Time > Time Scale

### Multi-instance learning with ML-Agents

To increase training speed, ML-Agents has implemented the ability for Agents to learn from multiple, parallel instances running simulatenously. 

Something like this:

![Screen Shot 2023-01-31 at 9.50.45 AM.png](CymStar%20Unity%20R&D%20Writeup%208a8cdf24d9aa4a8692890780e1da64a8/Screen_Shot_2023-01-31_at_9.50.45_AM.png)

For best results, we found that **16 instances** in a square formation (4x4) works best. Each instance is separated by 200 meters on the x or z axis, and has the Sketchfab models for the C5, KC135, and Boom Arm disabled to save rendering memory.

Here are some resources on the topic:

[ml-agents/Learning-Environment-Design.md at develop · Unity-Technologies/ml-agents](https://github.com/Unity-Technologies/ml-agents/blob/develop/docs/Learning-Environment-Design.md#multiple-areas)

[https://github.com/Unity-Technologies/ml-agents/issues/2028](https://github.com/Unity-Technologies/ml-agents/issues/2028)

### 👍 Improving Agent Performance

🎯 ******************************moving targets:******************************

Due to time constraints, we did not train the agent on a moving target. Additionally, we had a limited range of spawning positions for the C5. To improve the agent’s performance, I would add more possible spawning positions for the C5 as well as more approach routes so that the 

👁️ **removing ray perception sensors:**

The ray perception sensors on the Agent add a significant amount of data. This both slows down the network (more for it to think about) and slows down the sim (more data that needs to be collected each frame). Given more time to train, I believe that the Agent will learn the correct behavior without the ray perception sensors (or even by reducing the number of them). This was simply an issue due to time constraints. 

Here is information about next steps with our simulation and trained Agent

### 👬 Imitation Learning

Like we said in the presentation, a very exciting improvement for this project is to implement [Imitation Learning](https://smartlabai.medium.com/a-brief-overview-of-imitation-learning-8a8a75c44a9c). Given boom arm pilot data, the Agent could learn to imitate this data, effectively achieving as close-to-human performance as possible. A great thing for a training simulation to be!

Here is the documentation to implement Imitation Learning in ML-Agents:

[ML-agents/Training-Imitation-Learning.md at master · gzrjzcx/ML-agents](https://github.com/gzrjzcx/ML-agents/blob/master/docs/Training-Imitation-Learning.md)

### 📷 Camera Training

Another very exciting application of this project is to train an agent to accomplish Aerial Refueling using [Computer Vision](https://en.wikipedia.org/wiki/Computer_vision). Essentially, if one were to replace the inputs to the Agent (currently all the positions, fuel amount, etc) with a camera feed (flattened out to NxM pixels), the AI could learn to pilot the boom arm in REAL LIFE using Reward-Learning or Imitation Learning! Very Exciting. 

I recommend using a [CameraSensor](https://docs.unity.cn/Packages/com.unity.ml-agents@2.0/api/Unity.MLAgents.Sensors.CameraSensor.html?q=mlagents%20camerasensor) to accomplish this.

Here are some excellent videos on Computer Vision with ML-Agents:

[Camera Vision | Unity ML-Agents](https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwi22MzllfL8AhVxl2oFHWzmCzMQwqsBegQIDRAF&url=https%3A%2F%2Fwww.youtube.com%2Fwatch%3Fv%3D7FHyqzUBzZ0&usg=AOvVaw3OTnz_QHKYHNhQQIUNic2c)

[How to use Machine Learning AI Vision with Unity ML-Agents!](https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&cad=rja&uact=8&ved=2ahUKEwi22MzllfL8AhVxl2oFHWzmCzMQwqsBegQIDBAF&url=https%3A%2F%2Fwww.youtube.com%2Fwatch%3Fv%3DZV12uozR36k&usg=AOvVaw1Wm6sVN0yyROxaM33EuRA6)

and ways to improve its performance:

[Automotive and Manufacturing Onboarding - Machine Learning and Computer Vision](https://create.unity.com/atm-onboarding-machine-learning-and-computer-vision)


<!-- CONTACT -->
## Contact
### Isaac Green 
- isaac.green@holbertonstudets.com\
[![LinkedIn][linkedin-shield]][isaac-linkedin-url]

### Lyndon Pettersson
- 3266@holbertonstudets.com\
[![LinkedIn][linkedin-shield]][lyndon-linkedin-url]


Project Link: [https://github.com/IsaacGreenMachine/C5AerialRefueling](https://github.com/IsaacGreenMachine/C5AerialRefueling)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/IsaacGreenMachine/C5AerialRefueling.svg?style=for-the-badge
[contributors-url]: https://github.com/IsaacGreenMachine/C5AerialRefueling/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/IsaacGreenMachine/C5AerialRefueling.svg?style=for-the-badge
[forks-url]: https://github.com/IsaacGreenMachine/C5AerialRefueling/network/members
[stars-shield]: https://img.shields.io/github/stars/IsaacGreenMachine/C5AerialRefueling.svg?style=for-the-badge
[stars-url]: https://github.com/IsaacGreenMachine/C5AerialRefueling/stargazers
[issues-shield]: https://img.shields.io/github/issues/IsaacGreenMachine/C5AerialRefueling.svg?style=for-the-badge
[issues-url]: https://github.com/IsaacGreenMachine/C5AerialRefueling/issues
[license-shield]: https://img.shields.io/github/license/IsaacGreenMachine/C5AerialRefueling.svg?style=for-the-badge
[license-url]: https://github.com/IsaacGreenMachine/C5AerialRefueling/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[isaac-linkedin-url]: https://linkedin.com/in/-isaacgreen-
[lyndon-linkedin-url]: https://linkedin.com/in/lyndonpettersson
[refuel_im]: images/screenshot0.png
[sim_im]: images/screenshot1.png

[Python]: https://img.shields.io/badge/python-000000?style=for-the-badge&logo=python&logoColor=green
[Unity]: https://img.shields.io/badge/unity-000000?style=for-the-badge&logo=unity&logoColor=white
[MLAgents]: https://img.shields.io/badge/ml%20agents-000000?style=for-the-badge&logo=unity&logoColor=white
[PyTorch]: https://img.shields.io/badge/PyTorch-000000?style=for-the-badge&logo=pytorch&logoColor=red
[python-url]: https://www.python.org
[unity-url]: https://unity.com
[mla-url]: https://unity.com/products/machine-learning-agents
[pytorch-url]: https://pytorch.org
[refuel-video]: https://youtu.be/YPaLbtRKyTA

