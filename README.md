# MenuEngine
This is a simple framework that allows you to create and setup menues without codewriting. Fast and simple, it really helps to build menu system
based on pages, so you can test your game as fast as possible.

#Usage

Just create empty GameObject and attach 'MenuController' script. 

This system is based on pages, so you need to create different pages according to your menu structure. For example, you have the next pages:

Main, Lobby, Settings, Credits

You need to create empty GameObject as Canvas child for every page you want to add. Then, while creating elements for some page, make this elements
as children to the page object.

In Menu Controller you can add, remove and edit pages. Every page must have a special unique name. Also every page requires
a link to the page object you have alredy created.

The main engine is hidden in transitions between pages. You can set different parameters for every transition, such as animation and sound.
You can even add your custom animation.

If you try to set page that doesn't have transition from current page, new page just will be shown and previous won't be hidden.

