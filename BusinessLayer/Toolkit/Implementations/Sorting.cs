namespace RIACustomers.BusinessLayer.Toolkit;


public class Sorting {

    public static List<TypeToSort> BubbleSort<TypeToSort>( List<TypeToSort> Array, Func<TypeToSort, TypeToSort, Boolean> IsGreater ){

        var ClonedArray = new List<TypeToSort>( Array );

        for( Int32 OuterIndex = 0; OuterIndex < ClonedArray.Count -1; OuterIndex++ ){

            var HasSwaped = false;

            for( Int32 InnerIndex = 0; InnerIndex < ClonedArray.Count - 1 - OuterIndex; InnerIndex++ ){

                if( IsGreater( ClonedArray[InnerIndex], ClonedArray[InnerIndex + 1] ) ){
                    Swap( ClonedArray, InnerIndex, InnerIndex + 1 );
                    HasSwaped = true;
                }

            }

            if( !HasSwaped )
                return ClonedArray;

        }

        return ClonedArray;

    }

    private static void Swap<SortedType>(List<SortedType> Array, Int32 IndexA, Int32 IndexB){

        var Temp = Array[IndexA];
        Array[IndexA] = Array[IndexB];
        Array[IndexB] = Temp;

    }


}
