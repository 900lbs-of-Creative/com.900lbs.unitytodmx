# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.1.0] - 2020-7-15

### Added

- Unity to DMX Examples are now available as **LED Samples** in the Unity Package Manager in this package rather than as a separate repository.

### Changed

- Changed `ADMXController<T>` to no longer have a type constraint on the empty interface `IDMXProperties`.
- Changed `LEDDMXController` and `MultiLEDDMXController` to have properties of type `LEDEffect` instead of `LEDDMXProperties`.

### Removed

- Removed `LEDDMXProperties`.
- Removed `IDMXProperties`.

## [1.0.0] - 2020-3-31

### Added

- DMX controller objects that interface with DMX devices.
