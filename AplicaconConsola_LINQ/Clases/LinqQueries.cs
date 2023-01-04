using System.Linq;
public class LinqQueries
{
    private List<Book> librosCollection = new List<Book>();

    public LinqQueries()
    {


        using(StreamReader reader = new StreamReader("Libros/Libros.json"))
        {
            
            //vamos a guardar el contenido dentro de un string utilizando el metodo ReadToEnd 
            //para que lea el archivo de principio a fin

            string json = reader.ReadToEnd();

            //ahora vamos a tomar el texto y a convertirlo en una conleccion
            this.librosCollection = System.Text.Json.JsonSerializer.Deserialize<List<Book>>(json, new System.Text.Json.JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
        }
    }

    public IEnumerable<Book> TodaLaColeccion()
    {
        return librosCollection;
    }

    public IEnumerable<Book> librosDespues2000(){

        //con metodos de extension
        // return librosCollection.Where(libro => libro.PublishedDate.Year > 2000);


        //con query expression
        return from l in librosCollection where l.PublishedDate.Year > 2000 select l;
    }

    public IEnumerable<Book> dobleCondicion( int paginasMinimas, string palabraContenidaTitulo ){

        return librosCollection.Where(libro => libro.PageCount > paginasMinimas && libro.Title.Contains(palabraContenidaTitulo));
    }

    public bool verificacionStatus(){
        
        return librosCollection.All(libro => libro.Status != string.Empty);
    
    }

    public bool verificarPublicacionPorAnio(int anio){

        return librosCollection.Any(libro => libro.PublishedDate.Year == anio);

    }

    public IEnumerable<Book> verificacionCategoria(string categoria_buscada){
        
        return librosCollection.Where(libro => libro.Categories.Contains(categoria_buscada));

    } 

    public IEnumerable<Book> retornarOrdenado_filtrandoCategoria( string categoria ){
        
        List<Book> tmp = verificacionCategoria(categoria).ToList();
        
        return tmp.OrderBy(libro => libro.Title);
    }

    public IEnumerable<Book> paginasMinimas_Descendente( int pagMin ){
        
        return librosCollection.Where( libro => libro.PageCount > pagMin ).OrderByDescending( libro => libro.PageCount );
    }

    public IEnumerable<Book> seleccionTopPublicacionPorCategoria( string categoria, int top){
        
        return librosCollection.Where( libro => libro.Categories.Contains(categoria)).OrderByDescending(libro => libro.PublishedDate).Take(top);
    
    }

    public IEnumerable<Book> seleccionTercerYCuartoLibroMayorAPaginas( int minPag ){

        return librosCollection.Where(libro => libro.PageCount > minPag ).Take(4).Skip(2);

    }

    public void EjTakeWhile(){
        List<int> listado = new List<int>{1,2,3,4,5,6,7,8,9,10};

        List<int> result = listado.TakeWhile( numero => numero < 6).ToList();

        result.ForEach( resul => Console.WriteLine(resul));
        
    }

    public void EjSkipWhile(){
        List<int> listado = new List<int>{1,2,3,4,5,6,7,8,9,10};

        List<int> result = listado.SkipWhile( numero => numero < 6).ToList();

        result.ForEach( resul => Console.WriteLine(resul));
        
    }

    public IEnumerable<Book> TresPrimerosLibrosUsando_Select(){

        //se puede hacer una clase que guarde solo los elementos que solicitamos pero en este
        //caso vamos a crear nuevas instancias para retornar las cuales van a llevar solo los datos que necesitemos
        //retornar
        //Retornamos solo el titulo y la cantidad de paginas.

        return librosCollection.Take(3).Select( libro => new Book(){ Title = libro.Title, PageCount = libro.PageCount});
    }

    public int countLibrosPorNPaginas(int min, int max){
        
        return librosCollection.Count(libro => libro.PageCount >= min && max >= libro.PageCount);

    }

    public long countLibrosPorNPaginasLong(int min, int max){
        
        return librosCollection.LongCount(libro => libro.PageCount >= min && max >= libro.PageCount);

    }

    public DateTime menorFechaPublicacion(){

        return librosCollection.Min(libro => libro.PublishedDate);

    }

    public int mayorCantidadPaginasLibro(){

        return librosCollection.Max( libro => libro.PageCount);

    }

    public Book libroConMenorCantidadPaginasMayorQueCero(){

        return librosCollection.Where( libro => libro.PageCount >0 ).MinBy( libro => libro.PageCount );

    }

    public Book ultimoLibroPublicado(){

        return librosCollection.MaxBy( libro => libro.PublishedDate );

    }

    public int SumaPaginasLibrosConPaginasEntre_0y500(){
        return librosCollection.Where( libro => libro.PageCount >= 0 && libro.PageCount <= 500 ).Sum(libro => libro.PageCount);
    }

    public string TitulosDeLibrosDespuesDel2015Concatenados()
    {
        return librosCollection
                .Where(p=> p.PublishedDate.Year > 2015)
                .Aggregate("", (TitulosLibros, next) =>
                {
                    if(TitulosLibros != string.Empty)
                        TitulosLibros += " - " + next.Title;
                    else
                        TitulosLibros += next.Title;
                        
                    return TitulosLibros;
                });
    }

    public double PromedioCaracteresTitulo()
    {
        return librosCollection.Average(p=> p.Title.Length);
    }

    //para agrupar por anio
    public IEnumerable<IGrouping<int, Book>> LibrosDespuesdel2000AgrupadosporAnio()
    {
        return librosCollection.Where(p=> p.PublishedDate.Year >= 2000).GroupBy(p=> p.PublishedDate.Year);
    }
}


/*
Donde:

    >la opcion .Text.Json.JsonSerializer lo que hace es convertir el texto en un json que podemos manejar

    >luego lo convertimos en un tipo de dato definido por nosotros dependiendo lo que estemos reciviendo el json. En este caso queremos
    un listado de libros (es lo que viene en el json) "List<libro>" y le indicamos con que string debe trabajar en este caso es con el string
    llamado "json".

    >agregamos una propiedad mas para que las propiedades de cada objeto json se enlacen con las propiedades de la clase que definimos es decir 
    que ignore si vienen mayusculas o minusculas en las propiedades y que se enlace en sus distintas propiedades.

*/