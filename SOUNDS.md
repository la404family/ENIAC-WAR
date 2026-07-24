Programmeur Audio Expert en C# et Synthèse Sonore.

Objectif : Écrire le code C# complet d'un moteur audio procédural pour le jeu RTS NATIONS.SYS utilisant une esthétique rétro vectorielle (fond noir, lignes lumineuses, ambiance terminal militaire).

Consignes Techniques et Artistiques :

1. Zéro Fichier Audio : N'utiliser aucun fichier .wav, .mp3 ou .ogg. Tous les sons doivent être générés mathématiquement en temps réel (oscillateurs, filtres) par le code C#.
2. Zéro Voix Humaine : Aucune voix synthétisée (ni vocodeur, ni text-to-speech).
3. Zéro Musique de Fond : Aucune musique d'ambiance, aucun drone, aucune nappe synthétique. Seuls les effets sonores d'interface et d'événements sont présents.

4. Hiérarchie Sonore à 3 Niveaux :
- **Insignifiant** : Aucun son. Événements mineurs enregistrés silencieusement dans le log (ex: mouvement d'unités ennemies lointaines).
- **Normal** : Petit bip discret ou clic subtil. Événements courants (ex: point de ressource ramassé, unité déployée, territoire capturé).
- **Important** : Alarme sonore stridente, anxiogène (style DEFCON). Réservée aux événements critiques (ex: attaque de votre capitale, perte d'une unité, capitale ennemie détruite).

5. Sons d'Interface :
- Clics et bips très brefs/discrets pour les interactions souris (clic sur un pays, ouverture de menu, sélection d'une action).
- Bourdonnement (Hum) de fond optionnel pour l'immersion CRT (toujours très discret et non intrusif).
