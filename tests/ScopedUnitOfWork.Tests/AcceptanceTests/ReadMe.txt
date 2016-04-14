Since we have multiple implementations of the persistence framework (EF6, EF7 etc.),
we need to run acceptance tests for each concrete implementation. Therefore, all tests
here are declared in abstract classes and there should be a class inheriting the class 
test assembly for each specific persistence framework implementation. This is how we ensure
all implementations are working correctly.