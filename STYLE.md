Rôle : Expert en Programmation Graphique (Technical Artist) spécialisé en C# et rendu 2D géométrique.

Objectif : Écrire le code C# complet (orienté MonoGame) pour générer l'affichage visuel du jeu de stratégie géopolitique RTS ENIAC WAR. L'esthétique visée est celle d'un **écran de commandement militaire rétro** : fond très sombre (noir profond ou bleu nuit), lignes vectorielles lumineuses, courbes de niveau topographiques, typographie pixelisée/monospace. Le style doit évoquer une salle de guerre high-tech des années 80, pas un simple moniteur noir et blanc.

Consignes Techniques et Artistiques :

1. Zéro Asset Externe : Ne charger aucune image ou texture externe. Tout l'environnement visuel doit être généré mathématiquement par le code (primitives géométriques, lignes, polygones, courbes).

2. Palette de Couleurs :
- **Fond et Esthétique globale** : Noir absolu (0, 0, 0) avec un quadrillage discret (vert très sombre) en arrière-plan pour simuler un vieil écran cathodique (CRT) ou un terminal Minitel. Les textes principaux (UI, menus) s'affichent en Vert lumineux avec un curseur bloc clignotant.
- **Chaque pays possède une couleur distinctive** : vert, bleu, rouge, cyan. Les frontières, le nom du pays, du joueur et ses unités militaires s'affichent dans cette couleur.
- **Terrain** : Rendu en lignes de contour topographiques (courbes de niveau) jaune
- Les couleurs doivent lisible et distincte même à petite échelle. (plus sombre pour la topographie et le cadrillage

3. Carte Procédurale à Deux Couches :
- **Couche terrain (fixe)** : Générée au début de la partie via Perlin/Simplex. Représente la géographie physique vue de dessus : altitudes des terres, plaines, vallées. Le rendu utilise des **courbes de niveau topographiques** (lignes concentriques d'altitude) pour visualiser le relief, style carte d'état-major militaire.


Représentation des Entités sur la Carte :
- **Brouillard de guerre** : La structure de la carte est toujours visible, mais les unités ennemies n'apparaissent que dans le champ de vision des unités alliées.
- **Territoire et Expansion** : Lorsqu'une unité passe sur une case, celle-ci prend la couleur du joueur. La case conserve cette couleur tant qu'une unité ennemie n'y passe pas. Si deux couleurs différentes se rencontrent (frontière), la case de rencontre garde ou reprend sa couleur de base neutre.
- **Base / Capitale** : Le joueur commence avec une zone de départ d'environ 100 cases, centrée autour de sa "Capitale" (unique bâtiment du jeu). Cette zone de départ ne doit pas être un carré parfait (10x10), mais avoir des bordures irrégulières avec des décalages organiques. C'est depuis la Capitale que les unités apparaissent. Elle possède des points de vie non régénérables.
- **Ressources** : Des "points d'unités" apparaissent aléatoirement sur la carte dans les zones neutres/vides de présence de joueurs.
- **Unités (Militaires)** (max 150 par joueur, tir automatique, formes dans la couleur du joueur) :
  - **Infanterie (5 types)** : Mitrailleur, Soutien, Sniper, AntiTank, AntiAir.
  - **Chars (3 types)** : Anti-aérien, Anti-infanterie, Char lourd.
  - **Avions (2 types)** : Chasseur, Bombardier.

### Détail et Stats des Unités

#### 1. Infanterie (armure légère)
| Unité | PV | Dégâts | Cadence | DPS | Portée | Vitesse | Coût |
|---|---|---|---|---|---|---|---|
| Mitrailleur | 80 | 8 | 3/s | 24 | 4 | 6 | 50 |
| Soutien | 120 | 12 | 2/s | 24 | 5 | 4 | 75 |
| Sniper | 60 | 45 | 0,5/s | 22 | 9 | 3 | 90 |
| AntiTank | 90 | 80 (vs Char) | 0,4/s | 32 | 7 | 2 | 100 |
| AntiAir | 90 | 30 (air seul.)| 2/s | 60 | 8 | 3 | 110 |

#### 2. Chars (armure blindée)
| Unité | PV | Dégâts | Cadence | DPS | Portée | Vitesse | Coût |
|---|---|---|---|---|---|---|---|
| Char Anti-aérien | 150 | 25 (air seul.)| 3/s | 75 | 8 | 5 | 120 |
| Char Anti-infanterie| 200 | 15 (vs Inf.)| 3/s | 45 | 5 | 6 | 130 |
| Char lourd | 400 | 80 (zone) | 0,5/s | 40 | 6 | 2 | 220 |

#### 3. Avions (armure aérienne)
| Unité | PV | Dégâts | Cadence | DPS | Portée | Vitesse | Coût |
|---|---|---|---|---|---|---|---|
| Chasseur | 120 | 20 (air) / 5 (sol) | 2,5/s | 50 / 12| 5 | 12 | 140 |
| Bombardier | 180 | 100 (zone) | 0,4/s | 40 | 4 | 8 | 200 |

### Équilibrage : Qui tue qui ?
- **Mitrailleur** : Tue facilement l'infanterie légère (Soutien, Sniper). Peine contre chars, AntiTank. Ne cible pas les avions.
- **Soutien** : Efficace contre tout type d'infanterie. Peine contre chars lourds. Ne cible pas les avions.
- **Sniper** : Tue facilement infanterie isolée en 1 ou 2 tirs. Peine contre chars (dégâts trop faibles). Ne cible pas les avions.
- **AntiTank** : Tue facilement tous les chars (y compris lourd). Peine contre infanterie rapide (rate sa cible). Ne cible pas les avions.
- **AntiAir (infanterie)** : Tue facilement les Chasseurs. Peine contre Bombardiers (y arrive en groupe). Vulnérable à toute unité au sol (ne riposte pas au sol).
- **Char Anti-aérien** : Tue facilement Chasseurs/Bombardiers. Vulnérable au sol (AntiTank, Char lourd, Bombardier), il ne réplique pas au sol.
- **Char Anti-infanterie** : Tue facilement l'infanterie. Peine contre les autres chars. Vulnérable vs AntiTank, Char lourd.
- **Char Lourd** : Tue facilement l'infanterie groupée (zone) et les chars à l'usure. Lent et facilement contourné. Vulnérable vs AntiTank (bonus x2), Bombardiers, essaims de Mitrailleurs/Snipers.
- **Chasseur** : Tue facilement autres Chasseurs et Bombardiers (bonus anti-air). Correct contre sol (DPS réduit). Vulnérable vs AntiAir.
- **Bombardier** : Tue facilement infanterie groupée et chars (dégâts de zone énormes). Lent, cible très facile si l'ennemi a de l'AntiAir ou des Chasseurs. 
  - **Rendu** : Formes **pleines** (Filled) lors de la selection et sinon **creuses** 

