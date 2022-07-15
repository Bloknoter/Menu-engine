# MenuEngine
This is a simple framework that allows you to create and setup menues without codewriting. Fast and simple, it really helps to build menu system
based on pages, so you can test your game as fast as possible.

#Usage

Just create empty GameObject and attach 'MenuController' script. 

This system is based on pages, so you need to create different pages according to your menu structure. For example, you have the next pages:

Main, Lobby, Settings, Credits

You need to create empty GameObject as Canvas child for every page you want to add. Then, while creating elements for some page, make this elements
as children to the page object.

In Menu Controller you can add, remove and edit pages. Every page must have a special unique name, so it could be identified. Also every page requires
a link to the page object you alredy created.

The main engine is hidden in transitions. When you make a transition from one page to another, then you say controller to hide first page and show next.
If you try to set page that doesn't have a transition from current, this page will be shown, but previous page WON'T be closed. It's a good way to create
panels that don't have to close main interface, but must be closed after some work. Just don't create a transition from main page to this panel, but
add a transition from panel to main page, so it will be closed during this transition

Also there is an option to add some animations during the transitions, but I didn't really work on it) But you can create your custom animations deriving
from 'AnimationTransition' class.
