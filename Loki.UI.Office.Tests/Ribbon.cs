using System.Runtime.InteropServices;

using Microsoft.Office.Core;

// TODO:  suivez ces étapes pour activer l'élément (XML) Ruban :

// 1. Copiez le bloc de code suivant dans la classe ThisAddin, ThisWorkbook ou ThisDocument.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon();
//  }

// 2. Créez des méthodes de rappel dans la région "Rappels du ruban" de cette classe pour gérer les actions des utilisateurs
//    comme les clics sur un bouton. Remarque : si vous avez exporté ce ruban à partir du Concepteur de ruban,
//    vous devrez déplacer votre code des gestionnaires d'événements vers les méthodes de rappel et modifiez le code pour qu'il fonctionne avec
//    le modèle de programmation d'extensibilité du ruban (RibbonX).

// 3. Assignez les attributs aux balises de contrôle dans le fichier XML du ruban pour identifier les méthodes de rappel appropriées dans votre code.

// Pour plus d'informations, consultez la documentation XML du ruban dans l'aide de Visual Studio Tools pour Office.
namespace Loki.UI.Office.Tests
{
    [ComVisible(true)]
    public class Ribbon : LokiRibbon
    {
        public Ribbon()
        {
            RibbonXmlResource = "Loki.UI.Office.Tests.Ribbon.xml";
        }

        public void TestClick(IRibbonControl control)
        {
            PublishMessage<TestMessage>();
        }
    }
}