using System.Collections.Generic;

namespace Wiki.Models.Constants.Initializer
{
    public static class InitializerData
    {
        public const string AdminEmail = "admin@admin.com";
        public const string AuthorEmail = "autor@autor.com";
        public const string Author1Email = "autor1@autor.com";
        public const string Author2Email = "autor2@autor.com";
        public const string AuthorPassword = "Password1";

        public const string NbillanEmail = "nbillan@ort.edu.ar";
        public const string NbillanPassword = "hola1";

        public readonly static List<InitializerUser> Admins = new List<InitializerUser>
        {
            new InitializerUser
            {
                Email = AdminEmail,
                Password = Const.AdminPassword
            }
        };

        public readonly static List<InitializerUser> Authors = new List<InitializerUser>
        {
            new InitializerUser
            {
                Email = NbillanEmail,
                Password = NbillanPassword,
                FirstName = "Nicolás",
                LastName = "Billán"
            },
            new InitializerUser
            {
                Email = AuthorEmail,
                Password = AuthorPassword,
                FirstName = "Juan",
                LastName = "Pérez"
            },
            new InitializerUser
            {
                Email = Author1Email,
                Password = AuthorPassword,
                FirstName = "Camila",
                LastName = "Torres"
            },
            new InitializerUser
            {
                Email = Author2Email,
                Password = AuthorPassword,
                FirstName = "Alex",
                LastName = "Gómez"
            },
        };

        public const string CategoryAnimals = "Animales";
        public const string CategoryCities = "Ciudades";
        public const string CategorySports = "Deportes";

        public readonly static List<string> Categories = new List<string> { CategoryAnimals, CategoryCities, CategorySports };

        public const string KeyWordDomestic = "Domestico";
        public const string KeyWordCarnivore = "Carnivoro";
        public const string KeyWordOmnivore = "Omnivoro";
        public const string KeyWordMammal = "Mamifero";
        public const string KeyWordGoverment = "Gobierno";
        public const string KeyWordNational = "Nacional";
        public const string KeyWordArgentina = "Argetina";
        public const string KeyWordUnitedStates = "EstadosUnidos";
        public const string KeyWordSouthAmerica = "Sudamerica";
        public const string KeyWordNorthAmerica = "Norteamerica";
        public const string KeyWordEurope = "Europa";
        public const string KeyWordFrance = "Francia";

        public readonly static List<string> KeyWords = new List<string>
        {
            KeyWordCarnivore,
            KeyWordDomestic,
            KeyWordMammal,
            KeyWordOmnivore,
            KeyWordGoverment,
            KeyWordNational,
            KeyWordArgentina,
            KeyWordUnitedStates,
            KeyWordSouthAmerica,
            KeyWordNorthAmerica,
            KeyWordEurope,
            KeyWordFrance
        };

        public const string ArticleTitleCat = "Gato";
        public const string ArticleTitleDog = "Perro";
        public const string ArticleTitleBear = "Oso";
        public const string ArticleTitleBuenosAires = "Buenos Aires";
        public const string ArticleTitleNewYork = "Nueva York";
        public const string ArticleTitleParis = "París";

        public readonly static List<InitializerArticle> Articles = new List<InitializerArticle>
        {
            /* Gato */
            new InitializerArticle
            {
                Author = AuthorEmail,
                Title = ArticleTitleCat,
                Description = "El gato doméstico (Felis silvestris catus) es un mamífero carnívoro de la familia Felidae. Es una subespecie domesticada por la convivencia con el ser humano.",
                PrimaryCategory = CategoryAnimals,
                KeyWords = new List<string> { KeyWordDomestic, KeyWordCarnivore, KeyWordMammal },
                References = new List<string> { ArticleTitleDog },

                Entries = new List<InitializerEntry> {
                    new InitializerEntry
                    {
                        Title = "Atributos físicos",
                        SubTitle = "Peso",
                        Text = "Generalmente pesan entre 2,5 y 7 kg; sin embargo, algunas razas como el Ragdoll y el Maine Coon pueden exceder los 11,3 kilogramos. Han existido casos que superaron los 23 kg de peso debido a la sobrealimentación. El sobrepeso es perjudicial para el animal y debe ser evitado a través de una dieta equilibrada y ejercicio físico, especialmente en aquellos ejemplares exclusivamente hogareños."
                    },
                    new InitializerEntry
                    {
                        Title = "Orejas",
                        SubTitle = "Treinta y dos músculos individuales en la oreja le permiten oír direccionalmente",
                        Text = "Puede mover cada oreja independientemente de la otra. Gracias a esta capacidad, puede mover su cuerpo en una dirección y apuntar sus orejas en otra. La mayoría posee orejas rectas y erguidas"
                    },
                    new InitializerEntry
                    {
                        Title = "Metabolismo",
                        SubTitle = "Los gatos conservan la energía durmiendo más que cualquier otro animal",
                        Text = "Debido a su naturaleza nocturna, frecuentemente entran en un período de hiperactividad y alegría por la tarde, apodado vulgarmente como \"locura de la tarde\", \"locura de la noche\", \"la hora del gato loco\" o \"demencia de media hora\" por algunos científicos."
                    }
                }
            },

            /* Perro */
            new InitializerArticle
            {
                Author = AuthorEmail,
                Title = ArticleTitleDog,
                Description = "El perro (Canis familiaris o Canis lupus familiaris dependiendo de si se lo considera una especie por derecho propio o una subespecie del lobo), llamado perro doméstico o can; es un mamífero carnívoro de la familia de los cánidos, que constituye una especie del género Canis.",
                PrimaryCategory = CategoryAnimals,
                KeyWords = new List<string> { KeyWordDomestic, KeyWordCarnivore, KeyWordMammal },

                Entries = new List<InitializerEntry> {
                    new InitializerEntry
                    {
                        Title = "Características",
                        SubTitle = "Diferencias respecto a otros cánidos",
                        Text = "En comparación con lobos de tamaño equivalente, los perros tienden a tener el cráneo un 20 % más pequeño y el cerebro un 10 % más pequeño, además de tener los dientes más pequeños que otras especies de cánidos. Los perros requieren menos calorías para vivir que los lobos. Su dieta de sobras de los humanos hizo que sus cerebros grandes y los músculos mandibulares utilizados en la caza dejaran de ser necesarios."
                    },
                    new InitializerEntry
                    {
                        Title = "Pelaje",
                        SubTitle = "Al igual que los lobos los perros tienen un pelaje",
                        Text = "El pelaje de un perro puede ser un pelaje doble, compuesto de una capa inferior suave y una capa superior basta. A diferencia de los lobos, los perros pueden tener un pelaje único, carente de capa inferior."
                    },
                    new InitializerEntry
                    {
                        Title = "Aparato locomotor",
                        SubTitle = "El esqueleto ancestral de los perros les permite correr y saltar",
                        Text = "Como la mayoría de mamíferos predadores, el perro tiene músculos potentes, un sistema cardiovascular que permite una alta velocidad y una gran resistencia y dientes para cazar, aguantar y desgarrar las presas."
                    }
                }
            },

            /* Oso */
            new InitializerArticle
            {
                Author = Author1Email,
                Title = ArticleTitleBear,
                Description = "Los osos o úrsidos (Ursidae) son una familia de mamíferos omnívoros. Son animales de gran tamaño, y a pesar de su temible dentadura, comen frutos, raíces e insectos, además de carne.",
                PrimaryCategory = CategoryAnimals,
                KeyWords = new List<string> { KeyWordOmnivore, KeyWordMammal },

                Entries = new List<InitializerEntry> {
                    new InitializerEntry
                    {
                        Title = "Caracteristicas",
                        SubTitle = "Los osos se caracterizan por su cabeza de gran tamaño",
                        Text = "Los osos actuales miden entre 1 y 2,8 m de longitud total y tienen una masa de entre 27 y 780 kg (existen registros de machos de oso polar de alrededor de una tonelada). El macho suele ser un 20 % más grande que la hembra. El pelaje es largo y espeso, y generalmente de un solo color, a menudo marrón, negro o blanco. Como excepciones, el oso de anteojos tiene un par de círculos de pelo blanco rodeando los ojos y el oso panda, tiene un patrón de coloración blanco y negro bien definido."
                    },
                    new InitializerEntry
                    {
                        Title = "Etimología",
                        SubTitle = "La palabra inglesa bear proviene del inglés antiguo bera",
                        Text = "Los nombres de taxones de osos como Ursidae y Ursus provienen del latín Ursus (oso) / Ursa (osa). El primer nombre femenino \"Ursula\", originalmente derivado del nombre de un santo cristiano, significa \"pequeña osa\" (diminutivo del latín ursa). En Suiza, el nombre masculino \"Urs\" es especialmente popular, mientras que el nombre del cantón y la ciudad de Berna se deriva de Bär, oso en alemán."
                    },
                    new InitializerEntry
                    {
                        Title = "Comportamiento",
                        SubTitle = "Los osos son generalmente activos en su mayor parte durante el día",
                        Text = "Los osos son abrumadoramente solitarios y se los considera los más asociales de todos los carnívoros. Las únicas veces que se encuentran osos en grupos son las madres con crías u ocasionales generosidades estacionales de alimentos ricos (como los salmones). Pueden ocurrir peleas entre machos y los más adultos suelen tener cicatrices extensas, lo que sugiere que mantener el dominio puede ser intenso. Con su agudo sentido del olfato, los osos pueden localizar cadáveres a varios kilómetros de distancia. Utilizan el olfato para localizar otros alimentos, encontrar compañeros, evitar rivales y reconocer a sus cachorros."
                    }
                }
            },

            /* Buenos Aires */
            new InitializerArticle
            {
                Author = Author1Email,
                Title = ArticleTitleBuenosAires,
                Description = "Buenos Aires, en el texto de la Constitución: Ciudad de Buenos Aires o Ciudad Autónoma de Buenos Aires (CABA), también Capital Federal, por ser sede del gobierno nacional, es la capital y ciudad más poblada de la República Argentina.",
                PrimaryCategory = CategoryCities,
                KeyWords = new List<string> { KeyWordArgentina, KeyWordGoverment, KeyWordNational, KeyWordSouthAmerica },

                Entries = new List<InitializerEntry> {
                    new InitializerEntry
                    {
                        Title = "Geografía",
                        SubTitle = "Ubicación",
                        Text = "La ciudad de Buenos Aires se encuentra en Sudamérica, a 34° 36' de latitud sur y 58° 26' de longitud oeste, en la margen del Río de la Plata."
                    },
                    new InitializerEntry
                    {
                        Title = "Historia",
                        SubTitle = "Descubrimiento del territorio",
                        Text = "El 15 de enero de 1526, Diego García de Moguer, zarpó desde La Coruña, como capitán general de la armada, al mando de una expedición de tres naves, financiada por comerciantes, para buscar la ruta de las especias, siguiendo la derrota de Elcano, pasando por el estrecho de Magallanes. En el camino, en febrero de 1528, se detuvo a explorar la zona del Río de la Plata, por lo que se le atribuye su descubrimiento."
                    },
                    new InitializerEntry
                    {
                        Title = "Gobierno y administración",
                        SubTitle = "Gobierno Local",
                        Text = "El poder ejecutivo de la Ciudad Autónoma de Buenos Aires, denominado Gobierno de la Ciudad de Autónoma de Buenos Aires (GCABA) o Jefatura de Gobierno de la Ciudad de Autónoma de Buenos Aires, es ejercido por un jefe de gobierno electo por el voto popular en doble vuelta, cuya duración en el cargo es de cuatro años y con la posibilidad de reelección consecutiva por solamente un período más."
                    }
                }
            },

            /* Nueva York */
            new InitializerArticle
            {
                Author = Author1Email,
                Title = ArticleTitleNewYork,
                Description = "Nueva York (oficialmente New York City o NYC en siglas) es la ciudad más poblada de los Estados Unidos y una de las más pobladas del mundo.",
                PrimaryCategory = CategoryCities,
                KeyWords = new List<string> { KeyWordNorthAmerica, KeyWordUnitedStates, KeyWordGoverment, KeyWordNational },

                Entries = new List<InitializerEntry> {
                    new InitializerEntry
                    {
                        Title = "Geografía",
                        SubTitle = "Ubicación",
                        Text = "Nueva York está ubicada en el noreste de Estados Unidos, en el sureste del estado homónimo y aproximadamente a mitad de distancia entre Washington DC. y Boston. Su ubicación en la boca del río Hudson que forma un amplio puerto natural protegido que desemboca en el océano Atlántico, ha ayudado al crecimiento de la ciudad y a su importancia como ciudad comercial."
                    },
                    new InitializerEntry
                    {
                        Title = "Historia",
                        SubTitle = "Primeros asentamientos europeos",
                        Text = "En el momento de su descubrimiento europeo, en 1524 por Giovanni da Verrazzano, la región estaba habitada por alrededor de 5000 indígenas de la tribu de los Lenape. Este explorador italiano al servicio de la corona francesa la llamó Nouvelle Angoulême (Nueva Angulema). La instalación europea comenzó en 1614 en manos de los neerlandeses y en 1626, el jefe de la colonia, Peter Minuit, compró la isla de Manhattan a los Lenape (la leyenda, ahora refutada, cuenta que por abalorios de cristal por un valor de 24 dólares). El lugar sería renombrado como Nieuw Amsterdam y se especializaba en el comercio de pieles."
                    },
                    new InitializerEntry
                    {
                        Title = "Gobierno",
                        //SubTitle = "Gobierno Local",
                        Text = "Desde su consolidación en 1898, la ciudad de Nueva York ha sido una municipalidad metropolitana, con un sistema de gobierno liderado por un Alcalde y un Consejo de la ciudad. El gobierno es más centralizado que los de la mayoría de las demás ciudades estadounidenses."
                    }
                }
            },

            /* París */
            new InitializerArticle
            {
                Author = Author2Email,
                Title = ArticleTitleParis,
                Description = "París es la capital de Francia y su ciudad más poblada. Capital de la región de Isla de Francia (o \"Región Parisina\"), constituye el único departamento unicomunal del país.",
                PrimaryCategory = CategoryCities,
                KeyWords = new List<string> { KeyWordEurope, KeyWordFrance, KeyWordGoverment },

                Entries = new List<InitializerEntry> {
                    new InitializerEntry
                    {
                        Title = "Geografía",
                        SubTitle = "Ubicación",
                        Text = "París está situado en el norte de Francia, en el centro de la cuenca parisina. La ciudad es atravesada por el río Sena. En el centro de la ciudad destacan dos islas que constituyen su parte más antigua, Île Saint-Louis y la Isla de la Cité. En general, la ciudad es relativamente plana, y la altitud más baja es de 35 metros sobre el nivel del mar. Alrededor del centro de París destacan varias colinas, siendo la más alta Montmartre con 130 metros."
                    },
                    new InitializerEntry
                    {
                        Title = "Historia",
                        SubTitle = "Edad Antigua",
                        Text = "Los parisios, pueblo galo del que se deriva el nombre de París, dominaban el sector cuando las tropas de Julio César sitiaron el lugar. Se cree que los parisios fundaron la ciudad entre 250 a. C. y 200 a. C., aunque se desconoce el lugar exacto del emplazamiento de la ciudad gala; si bien, hay varios indicios que señalan que se establecieron en lo que hoy es la Ile de la Cité, sobre todo por razones de defensa estratégica al estar protegido el asentamiento por los brazos del río Sena que flanquean dicha isla fluvial."
                    },
                    new InitializerEntry
                    {
                        Title = "Cultura",
                        SubTitle = "Personas relevantes",
                        Text = "París ha sido un centro cultural y artístico relevante en la historia occidental. En ella nacieron, se formaron o desarrollaron sus carreras figuras francesas de la talla de René Descartes, Molière, Voltaire, Victor Hugo, Émile Zola, Alexandre Dumas, hijo, Edgar Degas y Claude Monet entre otros."
                    }
                }
            },
        };
    }
}
