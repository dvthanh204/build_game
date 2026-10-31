# Happy Hen Ranch — Game Design & Technical Specification

## 1. Product vision

**Genre:** cozy 3D ranch simulation with collectible creatures and short arcade activities  
**Platform:** Android first; iOS after device and store validation  
**Audience:** casual players who enjoy collection, decoration, gentle social play, and 2–10 minute sessions  
**Product promise:** build a colorful modern ranch, befriend distinctive chickens, and turn everyday care into a playful entertainment park.

The visual target is original, friendly stylized 3D: rounded silhouettes, readable colors, expressive animals, layered vegetation, warm lighting, and carefully controlled highlights. Use the named games in the brief as broad mood references only; production assets, characters, UI, and branding must be original.

### MVP product goals

| Goal | MVP proof |
|---|---|
| An immediately legible ranch | Player can move and identify coop, crops, chickens, and HUD within the first minute |
| A satisfying short loop | Pet/feed, collect an egg, plant, return to harvest |
| Touch-first comfort | Landscape layout, large controls, tap-to-move, no precision gestures required |
| Android-ready foundation | URP project, reproducible scene setup, Android build instructions |
| Honest production scope | Multiplayer, monetization, and content expansion are staged after the core loop is validated |

## 2. Core player loop and progression

```text
Explore ranch → care for chickens → collect eggs / grow crops
      ↑                                      ↓
  unlock areas ← earn coins, feed, seeds ← sell and harvest
      ↓
collect breeds → decorate ranch → enter minigames and seasonal events
```

### MVP interaction loop

1. Move to a chicken using a tap or the on-screen D-pad.
2. Pet for a small happiness increase or spend one grain to feed for a larger increase.
3. Visit the coop and collect available eggs for coins.
4. Plant a seed in a nearby bed. Crops mature over time; harvest returns grain and coins.
5. Spend rewards on future feed, seeds, cosmetics, and ranch expansions (future feature).

### Economy principles

- **Currencies:** soft currency (coins), production resources (grain/feed), seeds, and optional premium currency (future; cosmetic-first).
- Eggs and crops should be reliable, predictable sources of soft currency. Avoid random paid power.
- Paid items should prioritize cosmetics, convenience with transparent limits, and season-pass cosmetics.
- Rewarded ads must be opt-in, clearly labeled, frequency-capped, and never required to finish core progress.
- No paid loot boxes in the MVP. If randomized collection is later explored, disclose odds and comply with each market's regulations.

| Source | MVP reward | Sink / use |
|---|---:|---|
| Collect one egg | 8 coins | Decorations and ranch unlocks (future) |
| Harvest one crop bed | 5 coins + 2 grain | Feed chickens or seed crafting |
| Plant one bed | 1 seed | Crop progression |
| Feed one nearby chicken | 1 grain | Happiness and care progression |

These are prototype tuning values, not final live-economy values. Instrument playtests before balancing a persistent economy.

## 3. Creatures, breeding, and customization

### Chicken collection

Plan an initial launch roster of 24 authored breeds, expanding toward 100+ only when art production, memory budgets, and collection balance support it. Tiers describe visual rarity, not required combat power:

| Tier | Example direction | Acquisition |
|---|---|---|
| Common | Village hen, speckled hen | Tutorial, ranch progression |
| Uncommon | Sunflower, frosted, copper feather | Crafting and events |
| Rare | Pixel, deep-sea, stargazer | Minigame milestones |
| Legendary | Phoenix, clockwork, cosmic | Long-form event or transparent direct offer |

Breeding should be deterministic enough to plan: choose two eligible parents, preview possible trait families, and use earned incubation materials. Random mutation is a bonus, never a paid-only guarantee. Track lineage, cosmetic traits, and collection entries without making rare traits a power gate.

### Customization

- Chicken cosmetics: hats, glasses, scarves, wing effects, and emotes.
- Ranch themes: modern garden at launch, then western, storybook, and neon-night packs.
- Keep cosmetics modular and use shared skeletons/material atlases where possible.
- Offer colorblind-safe UI states and readable silhouettes at phone-screen scale.

## 4. Activities and social features

### Planned single-player activities

| Activity | Session | MVP status |
|---|---:|---|
| Ranch care and crops | 2–10 min | Implemented as prototype loop |
| Fishing pond | 3–5 min | Post-MVP; cast, wait, tension window, collection book |
| Golden egg maze | 5–8 min | Post-MVP; authored rooms, readable puzzles, checkpointing |
| Chicken kart race | 2–4 min | Post-MVP; lane/steer controls, jump, boost, ghost replay |

### Multiplayer strategy

Do not begin with a “thousands of players in one room” architecture. Prototype a small, capped social instance (for example 16–32 players), measure concurrency, and shard by region/event. Use authoritative server validation for trades, rewards, and progression. Separate the social plaza from the simulation save so ranch state remains available when the network is down.

| Feature | Suggested initial design |
|---|---|
| Town square | Instanced rooms, emotes, preset safe chat; text chat only after moderation tools |
| Visits | Asynchronous ranch snapshots first; opt-in cooperative session later |
| Weekly beauty show | Server-clocked submission window and rate-limited voting |
| Co-op tasks | Shared objectives with server-side reward validation |
| “Egg stealing” | Consent-based opt-in party minigame; never remove persistent items from a player's ranch |

Photon Fusion/Cloud or a server-authoritative custom service can be evaluated against expected concurrency, regions, persistence, moderation, and operating cost. Mirror is a networking library, not a turnkey hosted live-service backend.

## 5. Art direction, rendering, and audio

### Visual direction

- Stylized high-end 3D with soft forms, strong silhouettes, saturated but controlled palette, and original character design.
- Use physically based materials selectively: roughness and simple specular response add depth without requiring dense textures.
- Chicken “soft feathers” in the MVP use layered shapes and lighting. True real-time subsurface scattering is not a mobile baseline; approximate with warm backlighting, a soft rim, and carefully authored feather materials.
- Weather progression: sunny morning first; rain and snow are staged features. Avoid making wetness or snow dynamic material updates a prerequisite for the core loop.
- Harvest sparkle and campfire particles use short-lived, pooled particle systems. Provide reduced-effects settings.

### URP rendering targets

| Area | Target / approach |
|---|---|
| Rendering | URP Forward renderer; one main directional light; baked or mixed lighting for static props |
| Frame rate | 60 FPS target on tested upper-midrange devices; scalable 30 FPS fallback |
| Resolution | Dynamic render scale only after device profiling; preserve UI resolution |
| Shadows | One main caster, short distance, low/medium resolution on mobile |
| Materials | Shared materials, GPU instancing for repeated props, texture atlases |
| LOD | LOD0 near camera, simplified LOD1/2 farther away; cull hidden ranch zones |
| Crowds | Animation LOD, distance-based update rates, pooled actors; validate draw calls with profiler |
| Batching | Prefer SRP Batcher and GPU instancing; Dynamic Batching is not a universal win and must be profiled |
| Thermal/power | Profile 15–20 minute sessions on representative devices; reduce shadows, particles, and target FPS under thermal pressure |

The current sample has a small scene and does not demonstrate a 100-chicken stress test or certify a device frame-rate target.

### Sound and haptics

- Layer close chicken calls, feather/rustle, footsteps by surface, coop ambience, weather, and UI feedback.
- Use spatialized audio with distance attenuation for nearby animals; budget simultaneous voices and avoid continuous loud calls.
- Footstep material switches can be driven by ground type and animation events.
- Haptics: light tap for petting, short confirmation for egg collection, distinct but moderate fishing tension cue.
- Add mute, music/effects sliders, and disable-haptics settings before release.

## 6. Interaction and UX

- Landscape-first for ranch overview and thumb-friendly movement; evaluate portrait only if core navigation remains comfortable.
- Tap ground to move, visible on-screen D-pad fallback, contextual action buttons, generous touch targets (roughly 48 dp or larger).
- Camera follows at a stable angle with user-controlled zoom planned after the MVP.
- Show short, dismissible contextual feedback; never communicate essential progression by color alone.
- Tutorial sequence: move → pet → collect egg → plant → harvest → introduce collection book.
- Respect safe areas, Android back navigation, pause/resume, low-memory relaunch, and interrupted network connections.

## 7. Technical architecture

### Client

- Unity 2022.3 LTS + URP 14; C# gameplay components and ScriptableObject-driven definitions as content scales.
- Separate presentation, simulation, and persistence layers. The prototype currently combines setup and UI for rapid iteration; split these before production.
- Use Addressables only once asset scale warrants it; pool recurring particles, pickups, and NPCs.
- Save a versioned local profile with atomic writes and migration tests. Do not store purchase entitlements as client-trusted state.
- Add analytics with consent and data minimization; define events before instrumenting economy tuning.

### Backend decision

| Need | Candidate | Decision criteria |
|---|---|---|
| Authentication/profile/cloud save | PlayFab or Firebase | Platform identity, conflict resolution, data residency, cost |
| Real-time social session | Photon Fusion/Cloud or custom authoritative service | CCU pricing, regions, reconnect, authority, moderation |
| Push/remote config | Firebase or equivalent | Consent, frequency caps, staged rollout |
| Purchases | Google Play Billing via Unity IAP | Receipt validation, refunds, restore, regional compliance |

Never trust client-submitted currency, breed rolls, leaderboard scores, or purchase status. Validate valuable state server-side and retain an offline-friendly single-player path.

### Data and privacy

- Collect only data necessary for gameplay, support, safety, and service operation.
- Provide age-appropriate design, parental consent flows where required, deletion/export handling, and privacy disclosures before social or analytics features launch.
- Use moderated preset emotes by default; add chat only with reporting, blocking, retention policy, and staffed response processes.

## 8. Retention and monetization

- First-week journey: one short objective set per day, clear progress, and a meaningful cosmetic reward; never punish missed days.
- Seasonal Chicken Pass: free track plus optional premium cosmetic track. Avoid power-exclusive rewards and disclose season dates.
- Companion dog (Corgi/Shiba): cosmetic/following companion first; functional predator protection is a later feature and must not become a paid defense tax.
- Daily/weekly goals should reinforce care, exploration, and creative play—not excessive session frequency.
- Rewarded ads: voluntary bonus reroll/acceleration with caps; no forced interstitials during care or minigames.
- IAP: direct cosmetic bundles and pass; regional pricing, restore purchases, refund-safe entitlements, and purchase confirmation.

## 9. Production roadmap

| Phase | Duration estimate | Deliverables and exit criteria |
|---|---:|---|
| Prototype | 2–4 weeks | Movement, camera, one ranch loop, touch UX; test comprehension and fun with 10–20 players |
| Alpha | 8–12 weeks | Art direction, save/load, tutorial, 10–15 breeds, economy telemetry, Android device matrix; no blocker crashes |
| Beta / Soft launch | 8–12 weeks | Limited geography, remote config, support/moderation, store flow, privacy review, performance and retention cohorts |
| Official launch | 4–8 weeks preparation | Content calendar, localization, live ops runbooks, incident/rollback plans, store compliance, launch readiness review |

Indicative team for production: producer, game designer/economy designer, Unity client engineers, technical artist, 3D artists/animator, UI/UX designer, QA/device tester, and backend/live-ops support. Team size and dates must be re-estimated after prototype validation.

### Quality gates

1. Define a supported Android device matrix by GPU, memory, OS, and thermal tier.
2. Profile cold start, memory, draw calls, frame pacing, battery/thermal behavior, and interrupted sessions.
3. Test save migration, offline mode, cloud conflict, purchase receipt, and reconnect paths.
4. Run accessibility, privacy, localization, store policy, and moderation reviews before soft launch.
5. Gate every release with crash-free sessions and performance thresholds selected from actual device telemetry.

## 10. Current prototype limits

The Unity source in this repository is a playable visual/interaction skeleton using generated primitive geometry. It is not a production art build and does not contain cloud save, multiplayer, breeding, racing, fishing, maze, battle pass, ads, IAP, localization, or analytics. Unity Editor must generate the bootstrap scene on first open; an Android APK and device performance have not been verified in this environment.
