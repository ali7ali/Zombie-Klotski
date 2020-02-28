# Zombie Klotski
 Console program implementaion using smart algorithms DFS - BFS - Hill Climber

# Manual:
When running the game the number of rows and columns is required to be entered by the user
The game contains:
1- The option of playing from the user
The current patch is printed and the cursor position is indicated within the patch with []
The cursor is moved with the buttons Y-J-H-G
Y up - H down - J right - G left
When we want to move an element within the board (the element must be e-empty) we move the cursor to the element to be moved and then must press C for the check, then the available navigation directions for the element are printed, then we move the element directly with the buttons W-A-S-D
W up - S down - D right - A left
(The object should be moved immediately after testing with the C button)

2- Solve the game by DFS algorithm

3- Solve the game by BFS algorithm

4- Solve the game with the Hill Climber algorithm
Within the algorithm there are a couple of For loops
The first is for the first empty space
The second is for the second empty space
The same stack created for DFS is used to not repeat the definition of a new stack
There is a FindGoal function that displays the box coordinates that are located below the squares of the Big Zombies

5- Solve the game with an algorithm that return and tries all available moves for each element

