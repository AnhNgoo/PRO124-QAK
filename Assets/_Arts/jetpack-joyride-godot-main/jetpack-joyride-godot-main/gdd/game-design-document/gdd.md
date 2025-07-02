# JETPACK JOYRIDE - THE REMAKE (Game Design Document)

## Table of content

- [Overview](#overview)
	- [Skills](#skills)
- [Story](#story)
	- [Characters](#characters)
- [Gameplay](#gameplay)
	- [Gameplay Hook](#gameplay-hook)
	- [Loops](#loops)
	- [Entities](#entities)
	- [Progression](#progression)
	- [Losing](#losing)
- [Assets](#assets)
	- [Sensory Hook](#sensory-hook)
	- [Art](#art)
	- [Level Design](#level-design)
	- [Audio](#audio)
- [Technical](#technical)
	- [Platform](#platform)
	- [Tools](#tools)
- [Marketing & funding](#marketing--funding)
	- [Target](#target)
	- [Localization](#localization)

# Overview

Jetpack Joyride is a side-scrolling endless runner action video game. Join Barry as he breaks in to a secret laboratory to commandeer the experimental jetpacks from the clutches of science evildoers. After lift-off, simply touch the screen to ascend and release to descend, raining bullets, bubbles, rainbows and lasers downwards as you fly towards higher and higher scores!

[Great talk from the creators of the original Jetpack Joyride](https://www.gdcvault.com/play/1015527/Depth-in-Simplicity-The-Making)

## Skills

- Reaction
- Aim
- Speed

# Story

> Barry Steakfries breaks in to a secret laboratory, steals a machinegun jetpack from evil scientists and tries to fly away in order to do good.

## Characters

#### Barry Steakfries

> Works as a salesman for a gramophone-making company that is about to go bankrupt due to low sales.
> After seeing the machinegun jetpack he dreams of using it to do good.

#### Evil Scientists

> Working in the "top secret" laboratories of _Legitimate Research_.
> Researching and developing experimental weapons and vehicles.

# Gameplay

[_Gameplay from the original Jetpack Joyride_](https://www.youtube.com/watch?v=OhU7tLtOIgE)

## Gameplay Hook

Travel as far as possible, collect coins, and avoid hazards such as zappers, missiles and high-intensity laser beams.

When the player presses anywhere on the touchscreen, the jetpack fires and Barry rises.
When the player lets go, the jetpack turns off, and Barry falls.

Because he is continually in motion, the player does not control his speed, simply his movement along the vertical axis.

## Loops

Travel as far as you can without dying.
When you die you can play again to beat the highest score and use bonuses from previous games.

## Entities

### Player

By tapping/holding your finger to the screen, the player rises with the jetpack.
When no input is given the player falls down to the floor and runs.

The player always move from left to right automatically, and the speed increases while you play.

From the machinegun jetpack you fire bullets when flying, which can hit other entities.

Upon hitting obstacles, the player dies and tumbles across the floor like a ragdoll. With some luck you'll still be able to collect collectables and travel some more distance.

### Evil Scientists

They either walk or run on the labority floor.

They don't affect the player, but if they're hit by the player or explosions they will fall to the floor.

### Obstacles

- Zappers
- Missiles
- Lasers

### Collectables

- Coins
- Spin tokens

### Powerups

- Powerups can be bought one at a time or in packs (5 per pack)
- One type of powerup can only be used once per game
- They can either be purchased in or between games

#### Quick revive (heart)

- Effect: Revive the player so you can continue from where you died
- Use: Upon death
- Cost: Single: 1500; Pack: 6000
- Purchase: Upon death (single and used instantly) or in store

#### Final blast (bomb)

- Effect: Blasts the player so they tumble some additional distance before ending the game
- Use: Upon death
- Cost: Single: 1000; Pack: 4000
- Purchase: Upon death (single and used instantly) or in store

#### Head start

- Effect: 750 meters head start
- Use: At the start of a game
- Cost: Single: 500; Pack: 2000
- Purchase: Upon game start (single and used instantly) or in store

## Progression

Run endlessly from left to right while the speed increases.
The score increments by the distance you travel and the goal is to get the highest score possible.

## Losing

Contact with any of obstacles results in instant death.
Barry's body will tumble and slide for an additional distance upon dying.

The player can choose to use a powerup if they have any, like revival or a bomb, to either continue running or propel the player's body for additional distance. Only one type of powerup can be used per game though.

At the end of each run, the collected spin tokens are used in a slot machine (one token gives one spin) which can award the player various prizes, including coins, additional spin tokens, and powerups.

# Assets

## Sensory Hook

Juicy, punchy SFX, together with plenty of explosions and particle effects.

## Art

[Art Bible](../art-bible/art-bible.md)

## Level Design

[Level Design Document](../level-design-document/ldd.md)

## Audio

[Audio Design Document](../audio-design-document/add.md)

# Technical

## Platform

- Windows
- Mac
- Linux
- Web

The target resolution for the remake will is 1920x1080 (16:9).
It's then overridden to 1280x720 (16:9) by default.

## Tools

- Godot
- Git
- Github

# Marketing & funding

Promoted and available for free on:  
 [![Itch.io](https://img.shields.io/badge/Itch-%23FF0B34.svg?style=for-the-badge&logo=Itch.io&logoColor=white)](https://brallex.itch.io/)  
 [![youtube](https://img.shields.io/badge/youtube-FF0000?style=for-the-badge&logo=YouTube&logoColor=white)](https://youtube.com/@bbitofficial)  
 [![github](https://img.shields.io/badge/GitHub-000000?style=for-the-badge&logo=GitHub&logoColor=white)](https://github.com/Alexander-Jordan)  
 [![the 20 game challenge](https://img.shields.io/badge/The_20_Games_Challenge-205375?style=for-the-badge&logoColor=white)](https://20_games_challenge.gitlab.io/)  

## Target

Jetpack Joyride entusiasts and gamedevs.

## Localization

English
