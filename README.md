# Happy Hen Ranch — Unity URP Android MVP

This repository is a small, playable, touch-first 3D ranch prototype. It uses procedural placeholder art so the project can run without downloaded assets or credentials.

## Requirements

- Unity **2022.3.22f1** (or a compatible Unity 2022.3 LTS patch)
- Android Build Support, Android SDK & NDK Tools, and OpenJDK modules in Unity Hub
- Internet access on first project open so Unity Package Manager can resolve URP 14.0.11

## Open and play

1. Open this folder in Unity Hub with the Unity version above.
2. Wait for package resolution and compilation. The editor creates `Assets/Scenes/Ranch.unity` and adds it to Build Settings on first open.
3. Open `Assets/Scenes/Ranch.unity` and press **Play**.
4. Tap the ground to walk, or use the on-screen arrows. Get close to a chicken to pet/feed it; visit the coop to collect eggs and the crop beds to plant/harvest.

If automatic setup was interrupted, use **Chicken Ranch → Create or Repair Bootstrap Scene**.

## Build an Android APK

1. In Unity Hub, install the Android modules listed above.
2. In Unity, select **File → Build Settings → Android → Switch Platform**.
3. Confirm `Assets/Scenes/Ranch.unity` is enabled in Scenes In Build.
4. Select **Build** or **Build And Run**. The project is landscape and targets package ID `com.studio.happyhenranch`.

The APK has not been built or device-tested in this environment because Unity and Android build tools are not installed here.

## Build on GitHub Actions

The workflow at `.github/workflows/android-build.yml` builds an APK after each push to `main`, or when started manually from the repository's **Actions** tab. It uses a pinned Ubuntu 22.04 runner and GameCI Unity Builder v4.

1. You do not need to install the Unity Editor to build on GitHub Actions. For a Unity Personal license, use Unity Hub on any computer to activate your Unity account and create the `.ulf` license file. Installing Unity Hub alone is enough; the editor itself can remain installed only on the GitHub runner.
2. In the GitHub repository, open **Settings → Secrets and variables → Actions → New repository secret**.
3. Add these repository secrets: `UNITY_LICENSE` with the complete contents of the `.ulf` file, `UNITY_EMAIL` with your Unity account email, and `UNITY_PASSWORD` with your Unity account password. Do not commit the license file or put credentials in workflow YAML.
4. Open **Actions → Build Unity Android APK → Run workflow**, or push a change to `main`.
5. Download `agricultural-game-apk` from the completed workflow run's **Artifacts** section.

If you do not have a computer with Unity Hub, ask someone you trust to activate your Unity Personal license in Unity Hub and provide you the generated `.ulf` file securely; the license can be used by a build running on another operating system. Never share the `.ulf` file or Unity credentials in chat or commit them to the repository.

See the [official GameCI activation guide](https://game.ci/docs/github/activation/) for current license setup instructions.

## MVP scope

- Stylized, procedural 3D ranch, coop, feed mill, crop beds, fences, trees, and five chickens
- Tap-to-move and touch D-pad controls; follow camera; simple chicken idle/walk animation
- Pet/feed interactions, happiness values, timed egg spawning, collection, coins, planting, crop growth, and harvest rewards
- URP-compatible instanced materials, a 60 FPS target, and a simple landscape touch HUD

This is a foundation, not a production-ready live-service game. It does not yet include online multiplayer, accounts/cloud saves, ads/IAP, battle pass, production character art, localization, or device-specific performance validation.

See [GAME_DESIGN_VI.md](./GAME_DESIGN_VI.md) for the Vietnamese design specification, technical targets, economy, and roadmap. The [English design document](./GAME_DESIGN.md) is also available.
