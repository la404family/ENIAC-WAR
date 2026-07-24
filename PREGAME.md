# PREGAME.md — Écran de Préparation du Jeu (Lobby / Configuration)

> Spécification fonctionnelle et technique de la page de pré-partie pour **ENIAC WAR**.

---

## 1. Rôle et Objectif

La page de pré-partie (**Lobby / PreGameScreen**) est l'étape intermédiaire obligatoire entre le Menu Principal et le lancement de la simulation de combat en temps réel. Elle permet d'établir les conditions initiales du monde, la configuration des participants (Humains et IA), le paramétrage du réseau LAN et les règles de victoire.

Conformément à la direction artistique du projet, l'interface doit être intégralement rendue en **vectoriel pur (0 asset externe)** dans l'esthétique d'un **terminal de commandement militaire rétro (CRT / Minitel)**.

---

## 2. Modes d'Accès et Gestion Réseau LAN

La page de préparation s'adapte dynamiquement selon le mode sélectionné :

1. **Mode Solo (Vs IA)** :
   - L'utilisateur local est obligatoirement l'hôte (Slot 1).
   - Les slots 2, 3 et 4 sont configurables en IA ou désactivés.
   - Lancement immédiat au clic sur **LANCER L'ASSAUT**.

2. **Mode Multijoueur LAN — Hôte (Créer un salon)** :
   - Le serveur initialise la diffusion d'annonces UDP (**UDP Broadcast**) sur le réseau local.
   - L'hôte possède les droits exclusifs sur la configuration de la carte, des ressources et des conditions de victoire.
   - L'hôte peut verrouiller des slots, expulser des joueurs ou attribuer des slots IA.

3. **Mode Multijoueur LAN — Client (Rejoindre un salon)** :
   - Le client reçoit en temps réel la structure de données du salon (`LobbyStatePacket`).
   - Le client peut modifier son pseudo, sa couleur (si disponible) et son état **PRÊT**.
   - Les paramètres de carte et de victoire sont affichés en lecture seule.

---

## 3. Configuration des Slots Joueurs (Max 4 Joueurs)

Le jeu supporte jusqu'à **4 belligérants** simultanés. Chaque emplacement (slot) présente les options suivantes :

### Attributs d'un Slot
- **Type de Joueur** :
  - `HUMAIN` (Joueur connecté localement ou via LAN).
  - `IA FACILE` / `IA MOYEN` / `IA DIFFICILE` (Comportement automatique géré par FSM).
  - `OUVERT` (Attente d'un joueur LAN).
  - `FERMÉ` (Slot désactivé pour la partie).
- **Nom du Joueur / Faction** : Identifiant personnalisé (ex: `COMMANDANT_ALPHA`, `SECTEUR_01`).
- **Couleur Tactique (Exclusive)** :
  - `VERT` (0, 255, 65)
  - `BLEU` (0, 150, 255)
  - `ROUGE` (255, 50, 50)
  - `CYAN` (0, 255, 230)
  *Une couleur choisie par un joueur devient indisponible pour les autres.*
- **Indicateur d'État** :
  - `EN ATTENTE` (Texte jaune / clignotant).
  - `PRÊT` (Texte vert fixe + encadré lumineux).

---

## 4. Paramètres de Carte et de Simulation

Le salon permet de définir la génération du monde et les contraintes de jeu :

| Paramètre | Options Disponibles | Valeur par Défaut | Description |
|---|---|---|---|
| **Génération (Seed)** | Chiffre ou Aléatoire | Aléatoire | Graine de génération Perlin/Simplex pour les courbes topographiques. |
| **Brouillard de Guerre** | `ACTIVÉ` / `DÉSACTIVÉ` | `ACTIVÉ` | Masque les unités ennemies hors de portée de vision. |
| **Ressources Initiales** | `50` / `100` / `200` pts | `100 pts` | Points d'unités accordés au démarrage à chaque Capitale. |
| **Limite de Population** | `50` / `100` / `150` max | `150 max` | Nombre maximum d'unités actives par joueur. |
| **Vitesse de Jeu** | `x1.0` / `x1.5` / `x2.0` | `x1.0` | Multiplicateur du temps de simulation. |

---

## 5. Conditions de Victoire

L'hôte sélectionne la règle définissant la fin de la partie :

1. **Domination Totale (Par Défaut)** :
   - Élimination directe : La destruction de la Capitale d'un joueur le fait basculer en **Mode Spectateur**.
   - La victoire est attribuée au dernier joueur ou équipe survivante.

2. **Limite de Temps** :
   - Chronomètre défini (`10 min`, `20 min`, `30 min`).
   - À l'expiration du temps, le joueur possédant le **score le plus élevé** (cases contrôlées + unités abattues + bonus de Capitale) remporte la partie.

3. **Objectif de Points** :
   - Seuil de victoire défini (`1000 pts`, `2500 pts`, `5000 pts`).
   - Le premier joueur atteignant le score cible remporte immédiatement la victoire.

---

## 6. Interface Utilisateur Rétro & Console LAN (Layout)

L'écran s'organise en 4 blocs vectoriels distincts :

```text
+-----------------------------------------------------------------------------------+
|  ENIAC WAR — SALON DE COMMANDEMENT [LAN / SOLO]                    PING: 12ms     |
+------------------------------------------------------+----------------------------+
| EMPLACEMENTS JOUEURS                                 | PARAMÈTRES DE LA CARTE     |
|                                                      |                            |
| [1] HUMAIN  | CMD_ALPHA   | [VERT]  | [PRÊT]         | SEED    : 84920412         |
| [2] IA      | BOTE_DEF    | [BLEU]  | [PRÊT]         | BROUILLARD: ACTIVÉ         |
| [3] IA      | BOT_AGGRO   | [ROUGE] | [PRÊT]         | PTS DÉPART: 100 PTS        |
| [4] FERMÉ   | ---         | ---     | ---            | POP MAX   : 150 UNITES     |
|                                                      | VICTOIRE  : DOMINATION     |
+------------------------------------------------------+----------------------------+
| CONSOLE LAN & JOURNAL DU SALON                       | APERÇU TOPOGRAPHIQUE (SEED)|
|                                                      |                            |
| [SYSTEM] SALON CRÉÉ SUR 192.168.1.50:7777            |       /\   /\  .           |
| [JOIN] JOUEUR_2 A REJOINDA LA PARTIE.                |      /  \ /  \             |
| > TAPISSEZ VOTRE MESSAGE ICI...                      |     (  TOPOGRAPHIE  )      |
+------------------------------------------------------+----------------------------+
| [RETOUR MENU]                                            [LANCER L'ASSAUT (START)]|
+-----------------------------------------------------------------------------------+
```

---

## 7. Directives Architecturales C#

Lors de l'implémentation de la classe `PreGameScreen.cs` :
- **Implémenter `IScreen`** pour la gestion uniforme du cycle de vie (`Initialize`, `Update`, `Draw`).
- **Aucun Asset Externe** : Rendu des conteneurs, sélecteurs et boutons via `Renderer` (primitives et `SpriteFont`).
- **Respect de la Règle des ~250 Lignes** : Si la logique de réseau LAN ou de gestion de la console devient dense, la découper en composants modulaires (`LobbyNetworkHandler.cs`, `LobbyUIComponent.cs`).
- **Interdiction Absolue des Commentaires** : Code auto-explicatif par son typage et nommage.
