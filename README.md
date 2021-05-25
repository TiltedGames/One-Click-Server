<br /><br />


# What is this project?

This project aims to make the process of multiplayer integration and server set up just "one-click" away. It uses the [Web Socket protocol](https://en.wikipedia.org/wiki/WebSocket), implemented in C# using the .NET Sockets framework. To store game information, it uses [Redis](https://redis.io/).

<br />

It is being developed and tested alongside [Recoup](https://github.com/TwoBirdsGetStoned/Recoup). 

<br />

![Capture](https://user-images.githubusercontent.com/25698069/119578230-fd625980-bd70-11eb-8d0e-011a943b2646.PNG)


## How to run this server (Windows 32/64-bit)
1. If you're going to host online, choose a TCP port to host on (we use 5050)
2. Make sure the selected port is available to outside connections
   Quick guide for Windows...
   - Click start, and search for "Windows Defender Firewall"
   - Click "Advanced settings"
   - Click "Inbound rules"
   - Click "New Rule..."
   - Select "Port"
   - Select "Specific local ports"
   - Enter your chosen port (default is 5050)
   - Select "Create Rule"
3. Download this git repository as a .zip using the green code button.
4. Extract it wherever you'd like your server program to be stored
5. Run the 'gameserver.exe' from the extracted directory
6. Configure your settings, using the newly-opened port
7. Go back to the server program
8. Click 'Apply and Start Server'

<br />

## Requirements to rebuild (not necessary to run it!)
1. First make sure you have the following requirements:
   - Visual Studio [(the Community version is free)](https://visualstudio.microsoft.com/downloads/) or another C# IDE (such as IDEA)
   - .NET 3.5 framework (for running WebSocketServer)
   - [Redis installed](https://redis.io/topics/quickstart)
2. Once server files have been configured, simply open, modify, rebuild, and run the solution

<br />

## Understanding the project files

#### 1. ```/Gameserver/bin/Release/netcoreapp3.1/WebSocketServer.exe```

<details>
   <summary>Show/hide</summary>
   
   This is where the compiled program will be. Run this to start the server.
</details>

#### 2. ```/Gameserver/bin/Release/netcoreapp3.1/WebSocketServer.dll```
<details>
   <summary>Show/hide</summary>
   
   This is also made by VS but you shouldn't need to touch it. No need to link it to Unity, as it's separate from the client entirely. 
</details>

<br />

## Understanding the server files

#### 1. ```/Gameserver/Program.cs```

<details>
   <summary>Show/hide</summary>
   
   The main method of server is here. It just initializes network packages, sets up the server, and waits for input so the console doesn't close. 
</details>

#### 2. ```/Gameserver/Constants.cs```

<details>
   <summary>Show/hide</summary>
   Find constants like the server IP, port, and maximum connection queue size, here.
   
 </details>
 
 #### 3. ```/Gameserver/NetworkPackets.cs```
 
 <details>
   <summary>Show/hide</summary>
   
   The types of network packets we can send from client to server and server to client are defined here. Each is in its own enum. The integer associated each determines the handler function that is executed in ServerPackHandler.cs. 
 </details>
 
 #### 4. ```/Gameserver/ServerPacketHandler.cs```
 
 <details>
   <summary>Show/hide</summary>
   
   This file has our InitializeNetworkPackages method, which fills our dictionary assiociating our packets with their handler functions. It defines each packet handler (i.e. what to do with each type of packet it gets from the client). 
</details>

##### 5. ```/Gameserver/PacketBuffer.cs```

 <details>
   <summary>Show/hide</summary>
   
   This is out network packet buffer.
</details>

<br />

## Understanding the client files

<details>
   <summary>Show/hide</summary>
   
   #### 1. ```/Assets/Scripts/Network/WebSocketClient```

</details>

<br />

## What is actually on the network?

Answers to the whole universe.

<br />

## How is physics and user input handled?

Magic.

<br />
