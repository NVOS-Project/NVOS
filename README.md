# NVOS
This repository contains the NVOS framework running on the headset as well as (currently) all modules and services.

# Goals:
  - Provides night vision passthrough or other types of display to the end user
  - Provides an XR experience to help the user navigate their surroundings, control devices, etc.
  - Implements a natural way of interacting with the system
  - Provides a high level interface to the embedded firmware

# Implementation status and planned features:
  - ### NVOS.Core:
    - Core component registry: ✔️
    - Database backend: ✔️
    - Logger and log buffer: ✔️
    - Module manager: ✔️
    - Service manager: ✔️
  - ### NVOS.UI:
    - World UI: ✔️
    - Screen UI: ✔️
    - Quick tiles: ✔️
    - Window interactions: ✔️
    - World anchor reset: ❌
    - World 
    - Controls:
      - Button: ✔️
      - ButtonTile: ✔️
      - GridLayoutPanel: ✔️
      - HorizontalLayoutPanel: ✔️
      - HorizontalSlider: ✔️
      - Label: ✔️
      - Panel: ✔️
      - ScrollView: ✔️
      - Toggle: ✔️
      - ToggleTile: ✔️
      - VerticalLayoutPanel: ✔️
      - VerticalSlider: ✔️
  - ### NVOS.Network
    - RPC client: ✔️
    - RPC client error recovery: ✔️
    - RPC heartbeat service: ✔️
    - Capability / management APIs
      - Device reflection: ✔️
      - Network management: ❌
      - GPS: ❌
      - Compass: ❌
      - LED: ❌
