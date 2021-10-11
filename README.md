# Overview
![example workflow](https://github.com/GetFunkin/Funkin.NET/actions/workflows/build.yml/badge.svg)
![CodeFactor](https://www.codefactor.io/repository/github/getfunkin/funkin.net/badge)
![Repository size](https://img.shields.io/github/repo-size/getfunkin/funkin.net)
![Total lines of code](https://img.shields.io/tokei/lines/github/getfunkin/funkin.net)
![License](https://img.shields.io/github/license/getfunkin/funkin.net)
![Latest commit](https://img.shields.io/github/last-commit/getfunkin/funkin.net)

# Funkin.NET
Funkin.NET is an experimental, unfaithful, and heavily-W.I.P. Friday Night Funkin' port/fan-game for .NET 5 (C#), powered by osu!lazer's `osu.Framework`.
Most of the code here is either written by me, adapted from FNF's source code, or adapted from osu!lazer's code under the MIT license.

# Project Structures
| Project                            | Function                                                                                                                   |
|------------------------------------|----------------------------------------------------------------------------------------------------------------------------|
| `Funkin.NET.Desktop`               | Desktop host for `Funkin.NET`, works on Windows, MacOS, and Linux.                                                         |
| `Funkin.NET.Intermediary`          | Intermediate code that operates as a backend for `Funkin.NET` but does not use code exclusive to it. Meant to be reusable. |
| `Funkin.NET.Resources`             | Assembly holding resource files used in `Funkin.NET`.                                                                      |
| `Funkin.NET`                       | Main game assembly. Contains a majority of game functionality.                                                             |
| [`GetFunkin.AdobeNecromancer`][gf] | Sparrow Atlas parser used for managing Sparrow Atlas images and extracting frame data from them.                           |
| [`osu-framework`][of]              | Forked `osu-framework` sub-module that we rely on instead of the NuGet package so we can make small tweaks as needed.      |

[gf]: https://github.com/GetFunkin/GetFunkin.AdobeNecromancer
[of]: https://github.com/GetFunkin/osu-framework
