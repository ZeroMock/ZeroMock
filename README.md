# ZeroMock

ZeroMock is a mocking framework to mock concrete types.

It uses a syntax familier to `moq`

```cs
var mock = new Mock<MyConcreteClass>();

mock.Setup(e => e.Example(It.Is<string>(e => e.Contains("Hello"))))
    .Returns("World")
    .CallBack(() => Console.WriteLine("Callback!"));

var obj = mock.Object;
var result = obj.Example("Helloooo");

mock.Verify(e => e.Example("Hello"), Times.Once()
```

This project is in alpha stage, many other `moq` methods are not implemented.

This project cannot mock interfaces, it is recommended to use in combination with `moq` to combine feature sets.

---

### How to use:

- Mocking a concrete class will make all non-generic functions and properties return `default`.
- Fields are uneffected.
- ⚠️ Generics must be setup manually. If not, it will call the concrete behaviour.
- ⚠️ Methods that have been inlined by the JIT 

---

### Comparison against other mock frameworks:

| Name                                                        | Open Source | Interfaces | Concrete Types | Static | Production Ready |
| ----------------------------------------------------------- | :---------: | :--------: | :------------: | :----: | :--------------: |
| [Moq](https://github.com/moq/moq4)                          |      ✔️      |     ✔️      |       ❌        |   ❌    |        ✔️         |
| [TypeMock](https://www.typemock.com/isolator-product-page/) |      ❌      |     ✔️      |       ✔️        |   ✔️    |        ✔️         |
| [JustMock](https://www.telerik.com/products/mocking.aspx)   |      ❌      |     ✔️      |       ✔️        |   ✔️    |        ✔️         |
| [ZeroMock](https://github.com/CoenraadS/ZeroMock)           |      ✔️      |     ❌      |       ✔️        |   ❌    |        ❌         |