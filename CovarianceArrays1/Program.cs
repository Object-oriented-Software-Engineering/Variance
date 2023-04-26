// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

ProcessEntities pr = new ProcessEntities();
ObjectsRepository orep = new ObjectsRepository();
StringsRepository strrep = new StringsRepository();
pr.PrintEntities(orep);
pr.PrintEntities(strrep);


// EntitiesRepository<out T> plays the role  of the producer of objects of type T
// It cannot play the role of consumer of objects of type T
public interface EntitiesRepository<out T> {
    T Get(int index);
    int Length { get; }

    // ERROR : Invalid variance: The type parameter 'T' must be contravariantly
    // valid on 'EntitiesRepository<T>.PrintEntities(IEnumerable<T>)'.
    // 'T' is covariant and cannot be used as input argument to any method
    // of the interface
    //void PrintEntities(IEnumerable<T> entities);

    // EXCEPTION : Generic delegate as input parameter is allowed using
    // type parameter T 
    void PrintEntities(Action<T>  action);
}

// ObjectsRepository plays the role  of the producer of objects of type object
public class ObjectsRepository : EntitiesRepository<object> {
    List<object> m_entities = new List<object>(){"Hello","World"};
    public int Length => m_entities.Count;

    public object Get(int index) {
        return m_entities[index];
    }

    // ALLOWED because ObjectsRepository is not a generic type 
    public void PrintEntities(EntitiesRepository<string> entities) {
        for (int i = 0; i < entities.Length; i++) {
            Console.WriteLine($"Entity {i}:{entities.Get(i)}");
        }
    }

    public void PrintEntities(Action<object> action) {
        throw new NotImplementedException();
    }
}
// StringsRepository plays the role of the producer of objects of type string
public class StringsRepository : EntitiesRepository<string> {
    List<string> m_names= new List<string>(){"Greg","George"};
    public int Length => m_names.Count;
    public string Get(int index) {
        return m_names[index];
    }

    // ALLOWED 
    public void PrintEntities(EntitiesRepository<object> entities) {

    }

    public void PrintEntities(Action<string> action) {
        throw new NotImplementedException();
    }
}

// ProcessEntities plays the role of a consumer of objects
// ProcessEntities takes a collection of "objects" and since Liskov principle 
// holds PrintEntities will be able to process as well string objects that 
// are subtype of object and are returned from the producer object 
// EntitiesRepository<object>
// However ProcessEntities cannot call a method of EntitiesRepository<string>
// that takes string as an argument because it works with objects a less derived 
// type
public class ProcessEntities {

    public void PrintEntities(EntitiesRepository<object> entities) {
        for (int i = 0; i < entities.Length; i++) {
            Console.WriteLine($"Entity {i}:{entities.Get(i)}");
        }
    }

}
