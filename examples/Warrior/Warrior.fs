######################################################
##
##  Forth Warrior:
##
##  An educational game about introductory Forth
##  programming, basic AI concepts and stabbing
##  things with swords.
##
##  John Earnest
##
######################################################

:const start-level 0
:const delay-time  19

:include "../Print.fs"
:include "../Util.fs"
:include "../Sprites.fs"
:include "../Grid.fs"

:image   grid-tiles   "warriorTiles.png"  8  8
:image sprite-tiles "warriorSprites.png" 16 16

# directions
:const north 0
:const east  1
:const south 2
:const west  3

:data dir-x    0  1  0 -1
:data dir-y   -1  0  1  0

:data n "north"
:data e "east"
:data s "south"
:data w "west"
:data  dir-names n e s w
: .dir dir-names + @ type ;

# terrain types
:const empty  0
:const solid  1
:const stairs 2
:const gem    3
:const slime  4

######################################################
##
##  Level Data
##
######################################################

:data grid
:data l1
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2  6  7  6  7  6  7  6  7  6  7  2  2  2  2  6  7  6  7  6  7  6  7  2  2 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2 22 23 22 23 22 23 22 23 22 23  2  2  2  2 22 23 22 23 22 23 22 23  2  2 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2  8  9  0  1  0  1  0  1  0  1  2  2  2  2  0  1  0  1 12 13  0  1  2  2 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2 24 25 16 17 16 17 16 17 16 17  2  2  2  2 16 17 16 17 28 29 16 17  2  2 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2  2  2  2  2  2  2  2  2  0  1  2  2  2  2  0  1  2  2  2  2  0  1  2  2  2  2  2  2  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1  2  2  2  2  2  2  2  2  2  2 16 17  2  2  2  2 16 17  2  2  2  2 16 17  2  2  2  2  2  2  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1  2  2  0  1  6  7  6  7  0  1  2  2  2  2  0  1  6  7  6  7  6  7  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1  2  2 16 17 22 23 22 23 16 17  2  2  2  2 16 17 22 23 22 23 22 23  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1  2  2  0  1  0  1 10 11  0  1  2  2  2  2  0  1  0  1  0  1  4  5  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1  2  2 16 17 16 17 26 27 16 17  2  2  2  2 16 17 16 17 16 17 20 21  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2  2 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 
	-1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 -1 

:data levels l1

######################################################
##
##  Sprite Management
##
######################################################

:const player 200
:const icon   201

: alloc-sprite ( tile x y -- )
	32 for
		i sprite@ @ -if
			>r >r >r 16x16 r> r> r> r>
			>sprite exit
		then
	next
;

: free-sprites ( -- )
	32 for 0 i sprite@ ! next
;

######################################################
##
##  Game Simulation
##
######################################################

:proto tick
:proto level

:var move-dir
:var move-action
:var score

: wait
	delay-time for sync next
;

: stop
	"(Final score is " type score @ . ")" typeln
	loop keys if halt then sync again
;


: check-dir ( dir -- dir )
	0 over 3 within -if
		"Invalid direction: " type . cr stop
	then
;

: sprite-in-dir ( dir -- id )
	check-dir
	dup  dir-x + @ 16 * player px +
	swap dir-y + @ 16 * player py +
	32 for
		2dup i py = swap i px = and
		i sprite@ @ 0! and if
			2drop r> exit
		then
	next
	2drop 33
;

: tile-in-dir ( dir -- tile )
	check-dir
	dup  dir-x + @ 16 * player px +
	swap dir-y + @ 16 * player py +
	pixel-grid@ @
;

: show-dir ( tile dir -- )
	>r 16x16 swap
	i  dir-x + @ 16 * player px +
	r> dir-y + @ 16 * player py +
	icon >sprite wait icon hide
;

: init-level ( level -- )
	free-sprites
	dup level levels + @ GP !
	16x16 0 0 0 player >sprite
	20 for
		15 for
			j 2 * i 2 * tile-grid@ @
			dup 8 = if
				j 16 * player px!
				i 16 * player py!
			then
			dup 10 = if
				2 j 16 * i 16 * alloc-sprite
			then
			dup 12 = if
				3 j 16 * i 16 * alloc-sprite
			then
			drop
		next
	next
;

: no-action
	"No action specified." typeln stop
;

: walkable? ( tile -- flag )
	dup   0 =
	over  8 = or
	over 10 = or
	swap 12 = or
;

: make-walk
	move-dir @ sprite-in-dir tile
	3 = if
		drop
		drop 0 player sprite@ !
		"Devoured via stepping on a slime." typeln
		stop
	then
	move-dir @ tile-in-dir
	dup walkable? if
		drop
		move-dir @ dir-x + @ 16 * player px + player px!
		move-dir @ dir-y + @ 16 * player py + player py!
		"Walked " type move-dir @ .dir "." typeln
		exit
	then
	dup 4 = if
		drop 0 player sprite@ !
		"Completed the dungeon." typeln
		stop
	then
	drop
	"Cannot walk " type move-dir @ .dir "." typeln stop
;

: make-take
	move-dir @ sprite-in-dir tile
	dup 2 = if
		drop 0 move-dir @ sprite-in-dir sprite@ !
		1 player tile! wait 0 player tile!
		score @ 10 + score !
		"Picked up gem to the " type move-dir @ .dir "." typeln
		"(Score is now " type score @ . ")" typeln
		exit
	then
	drop
	"There's nothing " type move-dir @ .dir " to take." typeln stop
;

: make-attack
	move-dir @ sprite-in-dir tile
	dup 3 = if
		5 move-dir @ show-dir
		drop 0 move-dir @ sprite-in-dir sprite@ !
		"Killed slime to the " type move-dir @ .dir "." typeln
		exit
	then
	drop
	"There's nothing " type move-dir @ .dir " to attack." typeln stop
;

: main
	start-level init-level
	loop
		wait
		' no-action move-action !
		tick move-action @ exec
	again
;

######################################################
##
##  Controller API
##
######################################################

: walk   move-dir ! ' make-walk   move-action ! ; ( dir -- )
: take   move-dir ! ' make-take   move-action ! ; ( dir -- )
: attack move-dir ! ' make-attack move-action ! ; ( dir -- )

: look ( dir -- type )
	4 over show-dir
	dup sprite-in-dir tile
	dup 2 = if 2drop gem   exit then
	    3 = if  drop slime exit then
	tile-in-dir
	dup walkable? if drop empty  exit then
	    4 =       if      stairs exit then
	solid
;

# fill these in:
: level ( n -- ) drop ;

:var going-down
:var going-up

: tick
	going-up @ if
		north look empty = if north walk exit then
		false going-up !
	then

	going-down @ if
		south look empty = if south walk exit then
		false going-down !
	then

	east look
	dup gem    = if drop east take   exit then
	dup slime  = if drop east attack exit then
	    solid != if      east walk   exit then

	north look empty  = if north walk true going-up !   exit then
	south look empty  = if south walk true going-down ! exit then
;

