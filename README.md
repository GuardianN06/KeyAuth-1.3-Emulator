# KeyAuth 1.3 Emulator

**Disclaimer**: This emulator is **NOT** a bypass.

## Overview

This emulator replicates KeyAuth 1.3’s essential functionalities, allowing certain commands to work in an emulated environment. However, it doesn't bypass KeyAuth’s security and limitations as that is impossible due to ECC signing.

### Emulated Types

- **init** – Emulated
- **login** – Emulated
- **log** – Emulated
- **license** – Emulated
- **check** – Emulated
- **checkblacklist** – Emulated

### Planned Types (Not Yet Emulated)

- **file** – Not emulated (coming soon)
- **var** – Not emulated (coming soon)
- **webhook** – Not emulated (coming soon)

## Response Signing in KeyAuth 1.3

KeyAuth 1.3 adds a new layer of security by signing response messages. Each response is a combination of the epoch timestamp and the response body, signed using a private key. This signature is then verified with a public key.

- **Original KeyAuth Public Key**: `5586b4bc69c7a4b487e4563a4cd96afd39140f919bd31cea7d1c6a1e8439422b`

### Requirements for Emulation

To enable spoofing with this emulator, the public key in the KeyAuth code must be replaced. The emulator uses a new public key to sign messages.

- **Replacement Public Key**: `2571268f1826934a28a9eaa365c0496ac1e5a08bd23c4df275adf388948fd497`

Alternatively, you may add your own key pairs if you wish to.

## Key Pair Information

The emulator is configured to sign messages using a set of known public and private ed25519 keys, provided within the file for convenience.
