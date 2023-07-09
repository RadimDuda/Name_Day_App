# Hodnocení

Budu používat stupnici od 1 do 5. Vyšší hodnota je lepší výsledek. Jde tedy o tzv. "ruské" známkování opačné proti standardnímu českému :)

## Úkol 1

### DS
Vyspělejší řešení s lepším ošetřením chyb a použitím regulárního výrazu. Ošetření je na takové úrovni, že spolehlivě zpracuje libovolně poškozený soubor.

Z nějakého důvodu byly, ale provedeny změny na solution i projektu (NET6, přidán jiný projekt), což způsobuje konflikt pro merge.

Práce se souborem by mohla být elegantnější.

### RB
Řešení není dostatečně ošetřené na možné chyby v souboru. Stačí relativně malá chyba v datech a program havaruje a nezpracuje zbývající korektní údaje.

Parsování řádků pomocí "trimování" není zrovna ideální.

### Skóre

| Ukol | Hd. | DS    | RB      |
|------|-----|-------|---------|
|  1   | LF  | 4     | 2       | 
|  1   | VE  | 4     | 2       |
|  2   | LF  | 3     | 3       |
|  2   | VE  | 3     | 4       |
|  3   | LF  |       |         |
|  3   | VE  | 3     | 2       |

