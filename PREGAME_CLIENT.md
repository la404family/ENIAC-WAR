# PREGAME_CLIENT.md — Écran de Préparation : Mode Client LAN (ENIAC CLIENT)

> Spécifications de l'interface du joueur client rejoignant un salon LAN hébergé.

---

## 1. Rôle & Objectif

L'écran **PREGAME_CLIENT** est conçu pour les joueurs qui rejoignent une partie multijoueur sur le réseau local. Il se divise en 2 phases distinctes :
1. **Phase de Recherche / Connexion (Server Browser)** : Détection automatique des serveurs LAN via UDP Broadcast ou saisie d'IP.
2. **Phase de Salon Client (Lobby Reader/State)** : Configuration de son profil personnel (pseudo, couleur) et bascule de l'état **PRÊT / NON PRÊT**.

---

## 2. Phase 1 : Recherche & Détection des Serveurs LAN

Avant de rejoindre un salon, l'écran affiche la console de balayage réseau :

- **Écoute UDP (Port 7777)** : Capture des annonces émises par les serveurs `ENIAC HOTE` actifs.
- **Liste des Serveurs Détectés** :
  - Nom de l'hôte / Nom du salon.
  - IP et Port (`ex: 192.168.1.45:7777`).
  - Nombre de joueurs / Places libres (`ex: 2/4`).
  - Ping / Latence (en millisecondes).
- **Option Connexion Directe** : Saisie manuelle d'une adresse IP si le broadcast est bloqué par un pare-feu local.

---

## 3. Phase 2 : Salon Client (Attente & Préparation)

Une fois connecté au serveur de l'hôte, le client accède à l'interface de pré-partie :

- **Attributs Modifiables par le Client** :
  - Modification de son Nom de Commandant.
  - Sélection de sa Couleur parmi les couleurs encore libres.
  - Bouton Bascule **PRÊT / NON PRÊT** (READY / NOT READY).
- **Lecture Seule (Contrôlé par l'Hôte)** :
  - Les slots d'adversaires (Humains et IA).
  - Les paramètres de carte (Seed, ressources, brouillard de guerre).
  - Les conditions de victoire.

---

## 4. Layout Minitel CRT (Vectoriel Pur)

```text
+-----------------------------------------------------------------------------------+
|  ENIAC CLIENT — REJOINDRE LE SALON LAN                              PING: 8ms     |
+------------------------------------------------------+----------------------------+
| SALON REJOINT : SALON_ALPHA [HÔTE: 192.168.1.45]     | RÈGLES DE LA PARTIE (LECTURE)|
|                                                      |                            |
| [1] HÔTE   | COMMANDANT_1 | [VERT]  | [PRÊT]         | SEED      : 99401293       |
| [2] CLIENT | VOUS         | [BLEU]  | [PRÊT]         | BROUILLARD: ACTIVÉ         |
| [3] IA     | BOT_DEFENSE  | [ROUGE] | [PRÊT]         | PTS DÉPART: 100 PTS        |
| [4] OUVERT | ATTENTE...   | [CYAN]  | ---            | VICTOIRE  : DOMINATION     |
+------------------------------------------------------+----------------------------+
| CHAT TERMINAL LAN                                    | STATUT DE CONNEXION        |
|                                                      |                            |
| [SYSTEM] CONNECTÉ AU SALON DE COMMANDANT_1.          | CONNECTÉ A  192.168.1.45   |
| [CHAT] COMMANDANT_1: EN ATTENTE DU DERNIER JOUEUR... | LATENCE  : 8 ms            |
| > TAPISSEZ VOTRE MESSAGE...                          | STATUT   : EN ATTENTE...   |
+------------------------------------------------------+----------------------------+
| [SE DÉCONNECTER]                                         [JE SUIS PRÊT (TOGGLE)]  |
+-----------------------------------------------------------------------------------+
```

---

## 5. Bascule vers le Jeu (GameStartPacket)

En mode Client, le bouton de lancement n'existe pas. Dès que le paquet `GameStartPacket` est émis par l'Hôte, le Client bascule immédiatement sur `GameScreen` pour charger la carte synchronisée.
