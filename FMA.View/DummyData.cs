using System.Collections.Generic;
using FMA.Contracts;

namespace FMA.View
{
    public class DummyData
    {

        public static List<Material> GetDummyMaterials()
        {
            var materials = new List<Material>();

            var material1Fields = new List<MaterialField>
            {
                new MaterialField("Referent", "Arial", 10, false, false, false, 50, 1, 7, 71, "Paulus von Tarsus"),
                new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 20, "Titel 1"),
                new MaterialField("Untertitel", "Times New Roman", 12, true, false, true, 80, 3, 7, 15, "Untertitel 1"),
                new MaterialField("a", "Times New Roman", 12, true, false, true, 80, 3, 7, 15, "a 1"),
                new MaterialField("b", "Times New Roman", 12, true, false, true, 80, 3, 7, 45, "b 1"),
                new MaterialField("c", "Times New Roman", 12, true, false, true, 80, 3, 7, 55, "c 1"),
                new MaterialField("d", "Times New Roman", 12, true, false, true, 80, 3, 7, 65, "d 1")
            };

            var material1 = new Material(1, "Gef�hrlicher Glaube", "ZumThema Gef�hrlicher Glaube (Islamische Welt)", material1Fields);
            
            
            var material2Fields = new List<MaterialField>
            {
                new MaterialField("Referent", "Arial", 10, false, false, false, 10, 2, 7, 71, "Petrus"),
                new MaterialField("Titel", "Arial", 21, true, false, true, 50, 3, 7, 20, "Nordkorea - Gestern und Heute"),
                new MaterialField("Untertitel", "Times New Roman", 12, true, false, true, 80, 3, 7, 15, "Untertitel Nordkorea"),
                new MaterialField("Ort", "Times New Roman", 16, false, true, false, 80, 3, 7, 83, "Hamburg")
            };

            var material2 = new Material(2, "Nordkorea", "Zum Thema Nordkorea�", material2Fields);

            materials.Add(material1);
            materials.Add(material2);

            return materials;
        }
    }
}