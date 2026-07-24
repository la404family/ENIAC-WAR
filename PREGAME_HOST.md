# PREGAME_HOST.md — Écran de Préparation : Mode Hôte LAN (ENIAC HOTE)

> Spécifications de l'interface du créateur/hébergeur de partie sur le réseau local (LAN).

---

## 1. Rôle & Objectif

L'écran **PREGAME_HOST** est réservé au joueur qui crée le salon de combat sur le réseau local LAN. L'hôte possède l'autorité totale sur l'administration du salon : attribution des slots, règles de victoire, diffusion UDP du serveur et décision finale du lancement de la bataille.

---

## 2. Fonctionnalités d'Administration Hôte

L'hôte dispose d'outils de gestion en temps réel :

- **Diffusion Serveur (UDP Broadcast)** :
  - Émission automatique d'annonces UDP sur le port `7777` pour apparaître dans la liste des serveurs des clients LAN.
  - Affichage de l'adresse IP locale de l'hôte (`ex: 192.168.1.45`) et du port de connexion.
- **Gestion des Emplacements Joueurs (Slots 1 à 4)** :
  - Slot 1 : Réservé à l'hôte.
  - Slots 2, 3, 4 : Option de basculer chaque slot entre `OUVERT` (attente client LAN), `IA (FACILE/MOYEN/DIFFICILE)` ou `FERMÉ`.
  - Option d'**Expulser (KICK)** un client connecté sur un slot.
- **Paramétrage Strict de la Partie** :
  - Choix de la Seed, de la taille de carte, des ressources de départ et de la condition de victoire.
  - Tous ces paramètres sont verrouillés et diffusés aux clients via le paquet `LobbyStatePacket`.

---

## 3. Contrôle du Lancement (Bouton START)

Le bouton **LANCER L'ASSAUT (START)** est soumis à des règles de validation strictes :
1. Au moins **2 belligérants actifs** (Humain LAN ou IA) doivent être présents dans la partie.
2. Tous les joueurs humains connectés au salon doivent obligatoirement être au statut **PRÊT**.
3. En cas de non-respect, l'hôte reçoit un avertissement visuel rétro (`TOUS LES JOUEURS DOIVENT ÊTRE PRÊTS`).

---

## 4. Layout Minitel CRT (Vectoriel Pur)

```text
+-----------------------------------------------------------------------------------+
|  ENIAC HOTE — SALON LAN [IP: 192.168.1.45:7777]                     BROADCAST: ON |
+------------------------------------------------------+----------------------------+
| GESTION DES SLOTS (HÔTE)                             | RÈGLES ET PARAMÈTRES       |
|                                                      |                            |
| [1] HÔTE   | COMMANDANT_1 | [VERT]  | [PRÊT]         | SEED      : 99401293       |
| [2] CLIENT | PLAYER_TWO   | [BLEU]  | [PRÊT]  [KICK] | BROUILLARD: ACTIVÉ         |
| [3] IA     | BOT_DEFENSE  | [ROUGE] | [PRÊT]         | PTS DÉPART: 100 PTS        |
| [4] OUVERT | ATTENTE...   | [CYAN]  | ---            | VICTOIRE  : DOMINATION     |
+------------------------------------------------------+----------------------------+
| CONSOLE SERVEUR LAN                                  | APERÇU TOPOGRAPHIQUE       |
|                                                      |                            |
| [SYSTEM] SERVEUR OUVERT SUR 192.168.1.45:7777        |       /\   /\  .           |
| [JOIN] PLAYER_TWO A REJOINDA LE SLOT #2.             |      /  \ /  \             |
| [CHAT] PLAYER_TWO: PRET QUAND TU VEUX !              |     (  TOPOGRAPHIE  )      |
| > SAISIR MESSAGE SERVEUR...                          |                            |
+------------------------------------------------------+----------------------------+
| [FERMER SALON]                                           [LANCER L'ASSAUT (START)]|
+-----------------------------------------------------------------------------------+
```

---

## 5. Synchronisation Réseau LAN (Paquets)

L'hôte orchestre la communication via deux paquets majeurs :
- `LobbyStatePacket` (Fréquence: 2 Hz ou sur événement) : Transmet l'état complet du salon (slots, couleurs, états de préparation, paramètres de jeu).
- `GameStartPacket` : Transmis au clic sur START pour ordonner le chargement simultané du jeu chez tous les clients connectés.
