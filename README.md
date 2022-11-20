# HiLoGame
HiLo game

The principle of the game is as follows:
1. The system chooses a number between [Min; Max] (the mystery number)
2. The player proposes a number between [Min; Max]
3. If the player's proposal is not the mystery number, the system tells the player whether:

a. HI: the mystery number is > the player's guess

b. LO: the mystery number is < the player's guess

And the player plays again.

4. The goal of the game is to discover the mystery number in a minimum of iterations.


System Flow:

A - User get authenticathion token - JWT token;


B - User starts to play with the system:

	- first call will start the game, the user is associate with a mistery number and a new game is started. This is register in a List(fake DB table);

	- the user tries to guess the mistery number multiple times, while the user does not find the number, a few tips are returned from the system;

	- All user guess's are register into the system. This is register in a List(fake DB table);

	- when the user finds the number, the system returns a message saying that he got the right number and how many iterations were needed;


C - the user can play another new game or exit from the console app;


NOTE : The user cannot play the game without register and get a valid token from the server - Step A;


![Image alt text](Resources/hilo-flow.png?raw=true "HiLo game flow")